using System.Collections.Generic;
using Gameplay.Inventories;

namespace Gameplay.Crafting
{
    public static class CraftTool
    {
        public static bool CanCraft(IInventory inventory, CraftData craft)
        {
            foreach (var item in craft.Inputs)
            {
                if (!inventory.HasEnough(item.ItemId, item.Amount))
                    return false;
            }
            return true;
        }

        // returns ItemId and Amount
        public static IEnumerable<(int, int )> ExtractAllRequiredCraftItemsFromInventory(IInventory inventory, CraftData craft)
        {
            foreach (var item in craft.Inputs)
            {
                int removedAmount = inventory.Extract(item.ItemId, item.Amount);
                yield return (item.ItemId, removedAmount);
            }
        }
    }
}