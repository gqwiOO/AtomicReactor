using TMPro;
using UnityEngine;

namespace Gameplay.Map.Building
{
    public class ItemContainerCountView: MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _textField;

        private IItemContainer _itemContainer;

        public void Init(IItemContainer itemContainer)
        {
            if(_itemContainer != null)
                _itemContainer.OnAmountChanged -= ItemContainer_OnAmountChanged;
            
            _itemContainer = itemContainer;
            _itemContainer.OnAmountChanged += ItemContainer_OnAmountChanged;
            
            ItemContainer_OnAmountChanged(_itemContainer.Amount);

        }

        private void ItemContainer_OnAmountChanged(int newValue)
        {
            _textField.text = newValue.ToString();
        }
    }
}