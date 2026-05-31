using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using Gameplay.Transportation.ItemPipeSystem;
using UnityEngine;

namespace Gameplay.Map.Building.Chest
{
    public class ChestBuilding: BuildingMapObject, IItemExtractionSource, IItemInsertionTarget
    {
        public InventoryBuildingCore InventoryBuildingCore { get; private set; }
        
        public override async UniTask Init(Vector2Int cellPosition)
        {
            InventoryBuildingCore = new();
            base.Init(cellPosition);
        }

        public override void Tick()
        {
            
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            // TriggerUpdate();
        }

        public virtual void AddResource(int id, int amount = 1)
        {
            if (InventoryBuildingCore.Inventory.CanAdd(id, amount))
            {
                InventoryBuildingCore.Inventory.Add(id, amount);
            }
        }

        public bool CanAdd(int id, int amount = 1)
        {
            if (!IsWorking)
                return false;
            return InventoryBuildingCore.Inventory.CanAdd(id, amount);
        }

        public bool HasItemsForPipe()
            => InventoryBuildingCore?.Inventory.InventoryCells.Any(c => c.ItemId != -1 && c.Amount > 0) == true;

        public int GetExtractableItemId()
            => InventoryBuildingCore?.Inventory.InventoryCells
                .FirstOrDefault(c => c.ItemId != -1 && c.Amount > 0)?.ItemId ?? -1;

        public void ExtractForPipe(int itemId, int amount)
            => InventoryBuildingCore?.Inventory.Remove(itemId, amount);

        public bool CanInsertFromPipe(int itemId, int amount = 1)
            => IsWorking && InventoryBuildingCore?.Inventory.CanAdd(itemId, amount) == true;

        public void InsertFromPipe(int itemId, int amount)
            => InventoryBuildingCore?.Inventory.Add(itemId, amount);
    }
}