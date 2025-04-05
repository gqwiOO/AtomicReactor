using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Map.Building.Fluids;
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
        public List<IFluidProvider> FluidProviders { get; set; }
        
        public Dictionary<ICell, IFluidBuildingContainer> FluidBuildingContainers { get; set; }
        public IEnumerable<IPipe> Pipes => _pipes;
        public FluidType FluidType { get; set; } = FluidType.None;
        public float CurrentValue => FloatContainer.CurrentValue;

        public PipeSystem()
        {
            FloatContainer = new  FloatContainer();
            FluidProviders = new  List<IFluidProvider>();
            FluidBuildingContainers = new  Dictionary<ICell, IFluidBuildingContainer>();
            Key = GetHashCode();
        }
        
        public void AddFluidProvider(IFluidProvider fluidProvider)
        {
            FluidProviders.Add(fluidProvider);
            fluidProvider.OnAdded += FluidProvider_OnValueChanged;
        }
        
        private void UpdateFluidContainer(ICell cell, IFluidBuildingContainer fluidContainer)
        {
            if (fluidContainer == null)
                TryRemoveFluidContainer(cell,null);
            else
                TryAddFluidContainer(cell,fluidContainer);
        }

        private void TryAddFluidContainer(ICell cell, IFluidBuildingContainer fluidContainer) 
            => FluidBuildingContainers.TryAdd(cell, fluidContainer);
        private void TryRemoveFluidContainer(ICell cell, IFluidBuildingContainer fluidContainer) 
            => FluidBuildingContainers.Remove(cell);

        private void FluidProvider_OnValueChanged(FluidProvider fluidProvider)
        {
            if(FluidType != fluidProvider.FluidType && FluidType != FluidType.None)
                return;
            FluidType = fluidProvider.FluidType;
            
            FloatContainer.Add(fluidProvider.CurrentValue);
            fluidProvider.ExtractFluid(fluidProvider.CurrentValue);
            if (FloatContainer.CurrentValue > 0 && FluidBuildingContainers.Count > 0)
            {
                float singlePipeFluidValue = FloatContainer.CurrentValue / FluidBuildingContainers.Count;
                foreach (KeyValuePair<ICell, IFluidBuildingContainer> fluidContainersPair in FluidBuildingContainers)
                {
                    fluidContainersPair.Value.Add(singlePipeFluidValue);
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
            if (neighborCell.CellVisitor is WaterPumpMapObject waterPump)
            {
                AddFluidProvider(waterPump.FluidProvider);
                return;
            }

            if (neighborCell.CellVisitor is IFluidBuildingContainer fluidBuildingContainer)
            {
                UpdateFluidContainer(neighborCell,fluidBuildingContainer);
                return;

            }
            // if (neighborCell.CellVisitor == null)
            // {
                // IFluidBuildingContainer fluidContainer = neighborCell.CellVisitor as IFluidBuildingContainer;
                // UpdateFluidContainer(neighborCell,fluidContainer);
            // }
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

                foreach (IFluidProvider fluidProvider in pipeSystem.FluidProviders)
                {
                    AddFluidProvider(fluidProvider);
                }
                
                foreach (var (cell, fluidBuildingContainer) in FluidBuildingContainers)
                {
                    TryAddFluidContainer(cell,fluidBuildingContainer);
                }
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

                foreach (IFluidProvider fluidProvider in pipeSystem.FluidProviders)
                {
                    AddFluidProvider(fluidProvider);
                    
                }
                
                foreach (var (cell, fluidBuildingContainer) in FluidBuildingContainers)
                {
                    TryAddFluidContainer(cell,fluidBuildingContainer);
                }
                FloatContainer.Add(pipeSystem.CurrentValue);
                pipeSystem.Dispose();
            }

            return this;
        }

        public void Dispose()
        {
            _pipes = null;
            FluidBuildingContainers = null;
            foreach (IFluidProvider fluidProvider in FluidProviders)
            {
                fluidProvider.OnAdded -= FluidProvider_OnValueChanged;
            }
            FluidProviders = null;
        }
    }
}