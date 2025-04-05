using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building.Chest;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Furnace;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building
{
    public class ElectricFurnaceMapBuilding : BuildingMapObject, ISidesBuildingMapObject, IElectricBuildingCore
    {
        [field: SerializeField]
        public ElectricTwoItemsMechanismBuildingCoreCore ElectricTwoItemsMechanismBuildingCoreCore { get; private set; }

        private ChestBuilding _outputContainer;

        public BuildingSidesData BuildingSidesData => ElectricTwoItemsMechanismBuildingCoreCore.BuildingSidesData;

        public IElectricityContainer ElectricityContainer => ElectricTwoItemsMechanismBuildingCoreCore.ElectricityContainer;

        public override async UniTask Init(Vector2Int cellPosition)
        {
            var buildingSettings = _buildingsSettingsProvider.GetBuildingSettings(Key) as TwoItemsMechanismAsset;
            
            ElectricTwoItemsMechanismBuildingCoreCore = new ElectricTwoItemsMechanismBuildingCoreCore(buildingSettings?.TwoItemsMechanismSettingsData);
            
            ElectricTwoItemsMechanismBuildingCoreCore.OnItemProduced += ElectricTwoItemsMechanismBuildingCoreCoreOnItemProduced;

            BuildingSidesData.OnAnySideChanged += TriggerUpdate;
            base.Init(cellPosition);

            TryFindNeighbourContainer();
        }

        private void OnDestroy() => BuildingSidesData.OnAnySideChanged += TriggerUpdate;


        private void TryFindNeighbourContainer()
        {
            // TODO: 
        }

        private void ElectricTwoItemsMechanismBuildingCoreCoreOnItemProduced()
        {
            // TriggerUpdate();
            TryExtractOutput();
        }

        public override void Tick()
        {
            ElectricTwoItemsMechanismBuildingCoreCore.Tick(Time.deltaTime);    
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            SideType sideData = ElectricTwoItemsMechanismBuildingCoreCore.BuildingSidesData.GetSide(direction);

            TrySetOutputContainer(neighborCell, sideData);
            TrySetElectricityInputContainer(neighborCell, direction);
        }

        private void TrySetElectricityInputContainer(ICell neighborCell, Vector2Int direction)
        {
            ElectricTwoItemsMechanismBuildingCoreCore.CheckElectricityInput(neighborCell,direction);
        }

        private void TrySetOutputContainer(ICell neighborCell, SideType sideData)
        {
            if (sideData == SideType.Output && neighborCell.CellVisitor is ChestBuilding chestBuilding &&
                chestBuilding.CanAdd(ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.ItemId,
                    ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.Amount))
            {
                _outputContainer ??= chestBuilding;
                TryExtractOutput();
            }
        }

        private void TryExtractOutput()
        {
            if (_outputContainer && _outputContainer.CanAdd(ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.ItemId,ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.Amount))
            {
                _outputContainer.AddResource(ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.ItemId,
                    ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.Amount);
                
                ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.Extract(ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.Amount);
            }
        }
    }

    public interface ISidesBuildingMapObject
    {
        public BuildingSidesData BuildingSidesData { get; }
    }
}