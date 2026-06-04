using Gameplay.Inventories;
using Gameplay.Map.Building.Items.Provider;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Map.Building
{
    public class ItemContainerCountView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private Image _itemIcon;

        private SingleCellInventory _itemContainer;
        private IItemsDataProvider _itemsDataProvider;

        [Inject]
        private void Construct(IItemsDataProvider itemsDataProvider) => _itemsDataProvider = itemsDataProvider;

        public void Init(SingleCellInventory itemContainer)
        {
            if (_itemContainer != null)
                _itemContainer.OnChanged -= OnChanged;

            _itemContainer = itemContainer;
            _itemContainer.OnChanged += OnChanged;

            OnChanged();
        }

        private void OnDestroy()
        {
            if (_itemContainer != null)
                _itemContainer.OnChanged -= OnChanged;
        }

        private void OnChanged()
        {
            _textField.text = _itemContainer.Amount.ToString();

            if (_itemIcon != null && _itemContainer.Amount > 0)
            {
                var sprite = _itemsDataProvider.GetItemSprite(_itemContainer.ItemId);
                if (sprite) _itemIcon.sprite = sprite;
            }
        }
    }
}