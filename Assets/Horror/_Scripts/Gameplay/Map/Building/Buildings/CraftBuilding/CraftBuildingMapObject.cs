using Gameplay.Crafting;
using Gameplay.Inventories;
using Gameplay.Map.Building.Chest;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.CraftBuilding
{
    public class CraftBuildingMapObject : BuildingMapObject
    {
        private CraftBuildingCore _craftBuildingCore;
        private IItemStorage _outputContainerInventory;
        
        public IItemContainer ItemOutputContainer => _craftBuildingCore.ItemOutputContainer;
        public IInventory ItemsInputInventory => _craftBuildingCore.InputInventory;

        public override void Init(Vector2Int cellPosition)
        {
            _craftBuildingCore = new CraftBuildingCore();
            base.Init(cellPosition);
        }

        public override void Tick()
        {
            _craftBuildingCore.Tick(Time.deltaTime);
            TryExtractOutput(); 
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            _craftBuildingCore.OnNeighbourUpdated(neighborCell, direction);
            SideType sideData = _craftBuildingCore.BuildingSidesData.GetSide(direction);

            TrySetOutputContainer(neighborCell, sideData);
        }
        
        private void TrySetOutputContainer(ICell neighborCell, SideType sideData)
        {
            if (sideData == SideType.Output && neighborCell.CellVisitor is ChestBuilding chestBuilding)
            {
                if (chestBuilding.InventoryBuildingCore == null)
                    return;
                
                _outputContainerInventory ??= chestBuilding.InventoryBuildingCore.Inventory;
                TryExtractOutput();
            }
        }
        
        private void TryExtractOutput()
        {
            if (_craftBuildingCore.ItemOutputContainer == null)
                return;
            
            if (_craftBuildingCore.ItemOutputContainer.Amount == 0)
                return;

            if (_outputContainerInventory == null)
                return;
            int extractAmount = _craftBuildingCore.ItemOutputContainer.Amount;
            _craftBuildingCore.ItemOutputContainer.Extract(extractAmount);
            _outputContainerInventory.Add(_craftBuildingCore.ItemOutputContainer.ItemId, 
                extractAmount);
        }

        public void SetRecipe(CraftData craftData)
        {
            _craftBuildingCore.SetCraftData(craftData);
        }
    }
}
