using Gameplay.Map.Building.Items.Provider;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Inventories.Views
{
    public class InventoryCellView: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _countText;
        
        [SerializeField]
        private Image _itemIcon;
        
        private InventoryCell _cell;
        private IItemsDataProvider _itemsDataProvider;
        
        [Inject]
        private void Construct(IItemsDataProvider itemsDataProvider)
        {
            _itemsDataProvider = itemsDataProvider;
        }
        
        public void Init(InventoryCell cell)
        {
            if (_cell != null)
                _cell.OnCellUpdated -= UpdateView;
            _cell = cell;

            _cell.OnCellUpdated += UpdateView;
            
            UpdateView();
        }

        public void UpdateView()
        {
            var itemSprite = _itemsDataProvider.GetItemSprite(_cell.ItemId);
            if (itemSprite)
            {
                _itemIcon.sprite = itemSprite;
            }
            _countText.text = _cell.Amount.ToString();
        }
    }
}