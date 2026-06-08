using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building;
using Gameplay.Map.Building.Fluids.Tanks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public class FluidPipeSystem: IPipeSystem
    {
        private List<BasePipe> _pipes = new List<BasePipe>();
        public IFloatContainer FloatContainer { get; private set; }

        public int Key { get; private set; }
        public Dictionary<BasePipe, List<BuildingMapObject>> ConnectedBuildings { get; private set; }
        public Dictionary<ICell, IFluidContainer> FluidExtractionContainers { get; private set; }
        public Dictionary<ICell, IFluidContainer> FluidInsertionContainers { get; set; }
        public IEnumerable<BasePipe> Pipes => _pipes;
        public FluidType FluidType { get; set; } = FluidType.None;
        public float CurrentValue => FloatContainer.CurrentValue;

        public FluidPipeSystem()
        {
            FloatContainer = new FloatContainer();
            FluidExtractionContainers = new Dictionary<ICell, IFluidContainer>();
            FluidInsertionContainers = new Dictionary<ICell, IFluidContainer>();
            ConnectedBuildings = new Dictionary<BasePipe, List<BuildingMapObject>>();
            Key = GetHashCode();
        }

        private void TryAddFluidExtractionSource(ICell cell, IFluidContainer provider, IPipe pipe = null)
        {
            if (FluidExtractionContainers.TryAdd(cell, provider))
                provider.OnAdded += FluidProvider_OnValueChanged;
        }
        
        private void TryAddFluidMultiExtractionSource(BasePipe connectedPipe, BuildingMapObject building)
        {
            IFluidMultiExtractionSource fluidMultiExtractionSource = building as IFluidMultiExtractionSource;
            
            if (ConnectedBuildings.TryGetValue(connectedPipe, out var connectedBuildingsList))
            {
                connectedBuildingsList.Add(building); 
            }
            else
            {
                connectedBuildingsList = new List<BuildingMapObject>();
                connectedBuildingsList.Add(building);
                ConnectedBuildings.Add(connectedPipe, connectedBuildingsList);
            }
            
            
            // if (FluidExtractionContainers.TryAdd(connectedPipe.cell, fluidMultiExtractionSource))
                // provider.OnAdded += FluidProvider_OnValueChanged;
        }

        private void TryRemoveFluidSource(ICell cell)
        {
            if (FluidExtractionContainers.Remove(cell, out var provider))
                provider.OnAdded -= FluidProvider_OnValueChanged;
        }

        private void TryAddFluidInsertionContainer(ICell cell, IFluidContainer fluidContainer)
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
                IEnumerable<KeyValuePair<ICell, IFluidContainer>> validInsertionContainersCount = FluidInsertionContainers
                    .Where(item => item.Value.LockedFluidType == FluidType.None || item.Value.LockedFluidType == FluidType);
                float singlePipeFluidValue = FloatContainer.CurrentValue / FluidInsertionContainers.Count;
                
                foreach (KeyValuePair<ICell, IFluidContainer> pair in validInsertionContainersCount)
                {
                    pair.Value.AddFluid(fluidProvider.FluidType, singlePipeFluidValue);
                    FloatContainer.Remove(singlePipeFluidValue);
                }
            }
        }

        public void AddPipe(BasePipe pipe)
        {
            _pipes.Add(pipe);
            pipe.OnClicked += Pipe_OnClicked;
        }

        private void Pipe_OnClicked(BasePipe clickedPipe)
        {
            if (!ConnectedBuildings.TryGetValue(clickedPipe, out List<BuildingMapObject> connectedBuildingsList))
                return;
            foreach (BuildingMapObject building in connectedBuildingsList)
            {
                if (building is IFluidMultiExtractionSource fluidSource)
                {
                     KeyValuePair<ICell, IFluidContainer> currentActiveFluidContainer = 
                        FluidExtractionContainers.First(item => fluidSource.ExtractionFluidContainers.Contains(item.Value));

                     int currentIndex = fluidSource.ExtractionFluidContainers.IndexOf(currentActiveFluidContainer.Value);
                     int nextIndex = (currentIndex + 1) % fluidSource.ExtractionFluidContainers.Count;
                     IFluidContainer nextFluidContainer = fluidSource.ExtractionFluidContainers[nextIndex];

                     FluidExtractionContainers[currentActiveFluidContainer.Key] = nextFluidContainer;
                }
            }
        }

        public virtual void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            var connectedPipe = _pipes.FirstOrDefault(item => item.CellPosition == neighborCell.Position + direction);
            if (neighborCell.CellVisitor is IFluidExtractionSource fluidSource)
                TryAddFluidExtractionSource(neighborCell, fluidSource.ExtractionFluidContainer);

            if (neighborCell.CellVisitor is IFluidInsertionTarget fluidContainer)
                TryAddFluidInsertionContainer(neighborCell, fluidContainer.InsertionFluidContainer);

            if (neighborCell.CellVisitor is IFluidMultiExtractionSource fluidMultiSource)
            {
                TryAddFluidExtractionSource(neighborCell, fluidMultiSource.ExtractionFluidContainers[0]);
            }
            
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
                foreach (BasePipe pipe in pipeSystem.Pipes)
                {
                    pipe.UpdateParentSystem(this);
                    AddPipe(pipe);
                }

                foreach (var (cell, provider) in pipeSystem.FluidExtractionContainers)
                    TryAddFluidExtractionSource(cell, provider);

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
                foreach (BasePipe pipe in pipeSystem.Pipes)
                {
                    pipe.UpdateParentSystem(this);
                    AddPipe(pipe);
                }

                foreach (var (cell, provider) in pipeSystem.FluidExtractionContainers)
                    TryAddFluidExtractionSource(cell, provider);

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
