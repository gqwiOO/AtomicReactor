using Cysharp.Threading.Tasks;
using Gameplay.Crafting;
using Gameplay.Inventories;
using Gameplay.Map.Building.Chest;
using Gameplay.Map.Cell;
using Gameplay.Transportation.ItemPipeSystem;
using UnityEngine;

namespace Gameplay.Map.Building.CraftBuilding
{
    public class CraftBuildingMapObject : BuildingMapObject, IItemInsertionTarget
    {
        private CraftBuildingCore _craftBuildingCore;
        private IItemStorage _outputContainerInventory;
        
        public SingleCellInventory ItemOutputContainer => _craftBuildingCore.ItemOutputContainer;
        public IInventory ItemsInputInventory => _craftBuildingCore.InputInventory;

        public override async UniTask Init(Vector2Int cellPosition)
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
            // TrySetOutputContainer(neighborCell, sideData);
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
            int itemId = _craftBuildingCore.ItemOutputContainer.ItemId;
            _craftBuildingCore.ItemOutputContainer.Extract(itemId, extractAmount);
            _outputContainerInventory.Add(itemId, extractAmount);
        }

        public void SetRecipe(CraftData craftData)
        {
            _craftBuildingCore.SetCraftData(craftData);
        }

        public bool CanInsertFromPipe(int itemId, int amount = 1)
            => _craftBuildingCore?.InputInventory?.CanAdd(itemId, amount) == true;

        public void InsertFromPipe(int itemId, int amount)
            => _craftBuildingCore?.InputInventory?.Add(itemId, amount);
    }
}
