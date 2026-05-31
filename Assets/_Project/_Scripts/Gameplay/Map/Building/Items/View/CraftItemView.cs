using Gameplay.Inventories;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Map.Building.Items.View
{
    public class CraftItemView: MonoBehaviour
    {
        [SerializeField] private ItemView itemView;
        [FormerlySerializedAs("itemCountInInventory")] [SerializeField] private ItemCountInInventoryView itemCountInInventoryView;
        private IInventory _inventory;
        private int _itemId;
        private int _amount;

        public void Init(IInventory inventory,int itemId, int amount)
        {
            _amount = amount;
            _itemId = itemId;
            itemView.Set(itemId);
            
            _inventory = inventory;
            itemCountInInventoryView.Init(_inventory, _itemId);
            _inventory.OnChanged += Inventory_OnChanged;
        }

        private void Inventory_OnChanged()
        {
            itemView.SetAvailable(_inventory.HasEnough(_itemId,_amount));
        }
    }
}