using System;
using Gameplay.Map.Building.Items.Provider;
using Mechanics.Pools;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Items.View
{
    public class AllItemsPicker: MonoBehaviour
    {
        [SerializeField] public PoolGameObjects itemViewsPool;
        [SerializeField] public Transform container;
        
        private IItemsDataProvider _itemsDataProvider;

        private bool _isInited;
        public event Action<int> OnItemSelected;

        [Inject]
        private void Construct(IItemsDataProvider itemsDataProvider)
        {
            _itemsDataProvider = itemsDataProvider;
        }
        
        public void Init()
        {
            if (_isInited)
                return;
            itemViewsPool.Initialize();
            var items = _itemsDataProvider.GetAllItems();
            
            foreach (var item in items)
            {
                ItemView itemView = GetItemView();
                itemView.Set(item.ItemId);
                itemView.transform.SetParent(container);
                itemView.OnClicked += ItemView_OnClicked;
            }

            _isInited = true;
        }

        private void ItemView_OnClicked(int obj)
        {
            OnItemSelected?.Invoke(obj);
            Hide(); 
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private ItemView GetItemView()
        {
            ItemView itemView = itemViewsPool.Pull().GetOwner<ItemView>();
            itemView.gameObject.SetActive(true);
            return itemView;
        }
    }
}