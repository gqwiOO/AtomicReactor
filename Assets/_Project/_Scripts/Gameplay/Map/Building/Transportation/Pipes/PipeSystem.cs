using System.Collections.Generic;
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
        public Dictionary<ICell, IFluidProvider> FluidSources { get; private set; }
        public Dictionary<ICell, IFluidInsertionTarget> FluidBuildingContainers { get; set; }
        public IEnumerable<IPipe> Pipes => _pipes;
        public FluidType FluidType { get; set; } = FluidType.None;
        public float CurrentValue => FloatContainer.CurrentValue;

        public PipeSystem()
        {
            FloatContainer = new FloatContainer();
            FluidSources = new Dictionary<ICell, IFluidProvider>();
            FluidBuildingContainers = new Dictionary<ICell, IFluidInsertionTarget>();
            Key = GetHashCode();
        }

        private void TryAddFluidSource(ICell cell, IFluidProvider provider)
        {
            if (FluidSources.TryAdd(cell, provider))
                provider.OnAdded += FluidProvider_OnValueChanged;
        }

        private void TryRemoveFluidSource(ICell cell)
        {
            if (FluidSources.Remove(cell, out var provider))
                provider.OnAdded -= FluidProvider_OnValueChanged;
        }

        private void TryAddFluidContainer(ICell cell, IFluidInsertionTarget fluidContainer)
            => FluidBuildingContainers.TryAdd(cell, fluidContainer);

        private void TryRemoveFluidContainer(ICell cell)
            => FluidBuildingContainers.Remove(cell);

        private void FluidProvider_OnValueChanged(FluidProvider fluidProvider)
        {
            if (FluidType != fluidProvider.FluidType && FluidType != FluidType.None)
                return;
            FluidType = fluidProvider.FluidType;

            FloatContainer.Add(fluidProvider.CurrentValue);
            fluidProvider.ExtractFluid(fluidProvider.CurrentValue);
            if (FloatContainer.CurrentValue > 0 && FluidBuildingContainers.Count > 0)
            {
                float singlePipeFluidValue = FloatContainer.CurrentValue / FluidBuildingContainers.Count;
                foreach (KeyValuePair<ICell, IFluidInsertionTarget> pair in FluidBuildingContainers)
                {
                    pair.Value.Add(singlePipeFluidValue);
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
                TryAddFluidSource(neighborCell, fluidSource.ExtractionFluidProvider);

            if (neighborCell.CellVisitor is IFluidInsertionTarget fluidContainer)
                TryAddFluidContainer(neighborCell, fluidContainer);

            if (neighborCell.CellVisitor == null)
            {
                TryRemoveFluidSource(neighborCell);
                TryRemoveFluidContainer(neighborCell);
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

                foreach (var (cell, provider) in pipeSystem.FluidSources)
                    TryAddFluidSource(cell, provider);

                foreach (var (cell, container) in pipeSystem.FluidBuildingContainers)
                    TryAddFluidContainer(cell, container);

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

                foreach (var (cell, provider) in pipeSystem.FluidSources)
                    TryAddFluidSource(cell, provider);

                foreach (var (cell, container) in pipeSystem.FluidBuildingContainers)
                    TryAddFluidContainer(cell, container);

                FloatContainer.Add(pipeSystem.CurrentValue);
                pipeSystem.Dispose();
            }

            return this;
        }

        public void Dispose()
        {
            _pipes = null;
            FluidBuildingContainers = null;
            foreach (var (_, provider) in FluidSources)
                provider.OnAdded -= FluidProvider_OnValueChanged;
            FluidSources = null;
        }
    }
}
