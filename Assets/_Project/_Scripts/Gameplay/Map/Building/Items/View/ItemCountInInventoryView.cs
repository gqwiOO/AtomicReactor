using Gameplay.Inventories;
using TMPro;
using UnityEngine;

namespace Gameplay.Map.Building.Items.View
{
    public class ItemCountInInventoryView: MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        private IInventory _inventory;
        private int _itemId;

        public void Init(IInventory inventory, int itemId)
        {
            _itemId = itemId;
            _inventory = inventory;
            _inventory.OnChanged += Inventory_OnChanged;

            Inventory_OnChanged();
        }

        private void Inventory_OnChanged()
        {
            int itemCount = _inventory.GetItemCount(_itemId);
            if (itemCount == 0)
                Hide(); 
            else
                Show();
            
            Set(itemCount);
        }

        private void Hide()
        {
            gameObject.SetActive(false);   
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Set(int count)
        {
            textField.text = count.ToString();
        }

    }
}