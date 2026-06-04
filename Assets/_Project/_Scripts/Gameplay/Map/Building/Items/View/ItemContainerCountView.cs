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

        private IItemContainer _itemContainer;
        private IItemsDataProvider _itemsDataProvider;

        [Inject]
        private void Construct(IItemsDataProvider itemsDataProvider) => _itemsDataProvider = itemsDataProvider;

        public void Init(IItemContainer itemContainer)
        {
            if (_itemContainer != null)
                _itemContainer.OnAmountChanged -= OnAmountChanged;

            _itemContainer = itemContainer;
            _itemContainer.OnAmountChanged += OnAmountChanged;

            OnAmountChanged(_itemContainer.Amount);
        }

        private void OnDestroy()
        {
            if (_itemContainer != null)
                _itemContainer.OnAmountChanged -= OnAmountChanged;
        }

        private void OnAmountChanged(int newValue)
        {
            _textField.text = newValue.ToString();

            if (_itemIcon != null && newValue > 0)
            {
                var sprite = _itemsDataProvider.GetItemSprite(_itemContainer.ItemId);
                if (sprite) _itemIcon.sprite = sprite;
            }
        }
    }
}