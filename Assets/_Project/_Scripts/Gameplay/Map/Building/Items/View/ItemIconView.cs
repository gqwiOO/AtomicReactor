using Gameplay.Map.Building.Items.Provider;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Map.Building.Items.View
{
    public class ItemIconView : MonoBehaviour
    {
        private const float UNAVALIABLE_ALPHA = 0.5f;
        [SerializeField] private Image icon;

        private IItemsDataProvider _itemsDataProvider;

        [Inject]
        private void Construct(IItemsDataProvider itemsDataProvider)
        {
            _itemsDataProvider = itemsDataProvider;
        }

        public void Set(int itemId)
        {
            icon.sprite = _itemsDataProvider.GetItemSprite(itemId);
        }

        public void SetAvailableState(bool state)
        {
            if(state)
                icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1);
            else
                icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, UNAVALIABLE_ALPHA);
        }
    }
}