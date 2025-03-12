using Gameplay.Inventories.Views;
using Gameplay.Map.Building;
using Gameplay.Map.Building.Chest;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.MapUI.Views
{
    public class ItemsInventoryContainerView: BaseMapObjectView
    {
        [SerializeField] 
        private InventoryView inventoryView;

        public void SpecificInit(ChestBuilding buildingWithInventory)
        {
            inventoryView.Init(buildingWithInventory.InventoryBuildingCore.Inventory);
        }

        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as ChestBuilding);
        }
    }
}