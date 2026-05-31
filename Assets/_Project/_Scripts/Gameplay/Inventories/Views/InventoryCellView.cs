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
            if (_cell.Amount == 0)
            {
                _countText.gameObject.SetActive(false);
            }
            else
            {
                _countText.gameObject.SetActive(true);
            }

            var itemSprite = _itemsDataProvider.GetItemSprite(_cell.ItemId);
            if (itemSprite)
            {
                _itemIcon.sprite = itemSprite;
                _itemIcon.gameObject.SetActive(true);
            }
            else
                _itemIcon.gameObject.SetActive(false);
            
            _countText.text = _cell.Amount.ToString();
        }
    }
}