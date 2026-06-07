using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building.Fluids.Tanks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public abstract class PipeSystem: IPipeSystem
    {
        private List<IPipe> _pipes = new List<IPipe>();
        public IFloatContainer FloatContainer { get; private set; }

        public int Key { get; private set; }
        public Dictionary<ICell, IFluidContainer> FluidExtractionContainers { get; private set; }
        public Dictionary<ICell, IFluidInsertionTarget> FluidInsertionContainers { get; set; }
        public IEnumerable<IPipe> Pipes => _pipes;
        public FluidType FluidType { get; set; } = FluidType.None;
        public float CurrentValue => FloatContainer.CurrentValue;

        public PipeSystem()
        {
            FloatContainer = new FloatContainer();
            FluidExtractionContainers = new Dictionary<ICell, IFluidContainer>();
            FluidInsertionContainers = new Dictionary<ICell, IFluidInsertionTarget>();
            Key = GetHashCode();
        }

        private void TryAddFluidSource(ICell cell, IFluidContainer provider)
        {
            if (FluidExtractionContainers.TryAdd(cell, provider))
                provider.OnAdded += FluidProvider_OnValueChanged;
        }

        private void TryRemoveFluidSource(ICell cell)
        {
            if (FluidExtractionContainers.Remove(cell, out var provider))
                provider.OnAdded -= FluidProvider_OnValueChanged;
        }

        private void TryAddFluidInsertionContainer(ICell cell, IFluidInsertionTarget fluidContainer)
            => FluidInsertionContainers.TryAdd(cell, fluidContainer);

        private void TryRemoveFluidInsertionContainer(ICell cell)
            => FluidInsertionContainers.Remove(cell);

        private void FluidProvider_OnValueChanged(FluidContainer fluidProvider)
        {
            if (FluidType != fluidProvider.FluidType && FluidType != FluidType.None)
                return;
            FluidType = fluidProvider.FluidType;

            FloatContainer.Add(fluidProvider.CurrentValue);
            fluidProvider.ExtractFluid(fluidProvider.CurrentValue);
            if (FloatContainer.CurrentValue > 0 && FluidInsertionContainers.Count > 0)
            {
                IEnumerable<KeyValuePair<ICell, IFluidInsertionTarget>> validInsertionContainersCount = FluidInsertionContainers
                    .Where(item => item.Value.InsertionFluidContainer.LockedFluidType == FluidType.None || item.Value.InsertionFluidContainer.LockedFluidType == FluidType);
                float singlePipeFluidValue = FloatContainer.CurrentValue / FluidInsertionContainers.Count;
                
                foreach (KeyValuePair<ICell, IFluidInsertionTarget> pair in validInsertionContainersCount)
                {
                    pair.Value.Add(fluidProvider.FluidType, singlePipeFluidValue);
                    FloatContainer.Remove(singlePipeFluidValue);
                }
            }
        }

        public void AddPipe(IPipe pipe)
        {
            _pipes.Add(pipe);
        }

        public virtual void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            if (neighborCell.CellVisitor is IFluidExtractionSource fluidSource)
                TryAddFluidSource(neighborCell, fluidSource.ExtractionFluidContainer);

            if (neighborCell.CellVisitor is IFluidInsertionTarget fluidContainer)
                TryAddFluidInsertionContainer(neighborCell, fluidContainer);

            if (neighborCell.CellVisitor == null)
            {
                TryRemoveFluidSource(neighborCell);
                TryRemoveFluidInsertionContainer(neighborCell);
            }
        }

        public IPipeSystem CollapseSystems(params IPipeSystem[] pipes)
        {
            foreach (IPipeSystem pipeSystem in pipes)
            {
                foreach (IPipe pipe in pipeSystem.Pipes)
                {
                    pipe.UpdateParentSystem(this);
                    AddPipe(pipe);
                }

                foreach (var (cell, provider) in pipeSystem.FluidExtractionContainers)
                    TryAddFluidSource(cell, provider);

                foreach (var (cell, container) in pipeSystem.FluidInsertionContainers)
                    TryAddFluidInsertionContainer(cell, container);

                FloatContainer.Add(pipeSystem.CurrentValue);
                pipeSystem.Dispose();
            }

            return this;
        }

        public IPipeSystem CollapseSystems(List<IPipeSystem> pipes)
        {
            foreach (IPipeSystem pipeSystem in pipes)
            {
                foreach (IPipe pipe in pipeSystem.Pipes)
                {
                    pipe.UpdateParentSystem(this);
                    AddPipe(pipe);
                }

                foreach (var (cell, provider) in pipeSystem.FluidExtractionContainers)
                    TryAddFluidSource(cell, provider);

                foreach (var (cell, container) in pipeSystem.FluidInsertionContainers)
                    TryAddFluidInsertionContainer(cell, container);

                FloatContainer.Add(pipeSystem.CurrentValue);
                pipeSystem.Dispose();
            }

            return this;
        }

        public void Dispose()
        {
            _pipes = null;
            FluidInsertionContainers = null;
            foreach (var (_, provider) in FluidExtractionContainers)
                provider.OnAdded -= FluidProvider_OnValueChanged;
            FluidExtractionContainers = null;
        }
    }
}
