using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building.Chest;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Furnace;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Cell;
using Gameplay.Transportation.ItemPipeSystem;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building
{
    public class ElectricFurnaceMapBuilding : BuildingMapObject, ISidesBuildingMapObject, IElectricBuildingCore, IItemExtractionSource
    {
        [field: SerializeField]
        public ElectricTwoItemsMechanismBuildingCoreCore ElectricTwoItemsMechanismBuildingCoreCore { get; private set; }
        
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
        }

        public override void Tick()
        {
            ElectricTwoItemsMechanismBuildingCoreCore.Tick(Time.deltaTime);    
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            TrySetElectricityInputContainer(neighborCell, direction);
        }

        private void TrySetElectricityInputContainer(ICell neighborCell, Vector2Int direction)
        {
            ElectricTwoItemsMechanismBuildingCoreCore.CheckElectricityInput(neighborCell,direction);
        }

        public bool HasItemsForPipe()
        {
            return ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.HasEnough(1);
        }

        public int GetExtractableItemId()
        {
            return ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer.ItemId;
        }

        public void ExtractForPipe(int itemId, int amount)
        {
            ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer?.Extract(itemId, amount);
        }
    }

    public interface ISidesBuildingMapObject
    {
        public BuildingSidesData BuildingSidesData { get; }
    }
}