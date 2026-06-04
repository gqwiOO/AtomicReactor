using System.Linq;
using Gameplay.Inventories;
using Gameplay.Transportation.ItemPipeSystem;

namespace Gameplay.Map.Building
{
    public abstract class BaseItemMinerMapObject : BuildingMapObject, IItemExtractionSource
    {
        protected abstract IInventory Storage { get; }
        public abstract int MinedItemId { get; }
        public abstract float MiningRatePerSecond { get; }

        public InventoryCell InventoryCell => Storage?.InventoryCells.FirstOrDefault();

        public bool HasItemsForPipe()
            => InventoryCell != null && InventoryCell.ItemId != -1 && InventoryCell.Amount > 0;

        public int GetExtractableItemId()
            => InventoryCell?.ItemId ?? -1;

        public void ExtractForPipe(int itemId, int amount)
            => Storage?.Remove(itemId, amount);
    }
}
