using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Fluids.Tanks;
using Gameplay.Map.Cell;
using Gameplay.Transportation.WaterPipeSystem;
using UnityEngine;

namespace Gameplay.Map.Building.ElectrolyticSeparator
{
    public class ElectrolyticSeparatorBuilding: BuildingMapObject, IFluidMultiExtractionSource, IFluidInsertionTarget, IElectricBuildingCore
    {
        private ElectrolyticSeparatorCore _core;
        public IFluidContainer ExtractionFluidContainer_1 => _core.Output1FluidContainer;
        public IFluidContainer ExtractionFluidContainer_2 => _core.Output2FluidContainer;
        public IFluidContainer InsertionFluidContainer => _core.InputFluidContainer;
        public IElectricityContainer ElectricityContainer => _core.ElectricityContainer;
        public List<IFluidContainer> ExtractionFluidContainers { get; private set;  }
        public override async UniTask Init(Vector2Int cellPosition)
        {
            await base.Init(cellPosition);
            _core = new ElectrolyticSeparatorCore(_buildingsSettingsProvider.GetBuildingSettings(Key) as ElectrolyticSeparatorSettingsData);

            ExtractionFluidContainers = new List<IFluidContainer>()
                { _core.Output1FluidContainer, _core.Output2FluidContainer };
        }

        public override void Tick()
        {
            _core.Tick(Time.deltaTime);
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            
        }

    }
}