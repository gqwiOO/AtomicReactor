using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private bool selectableViews;

        [SerializeField] private bool hideOnSelect;
        
        private IItemsDataProvider _itemsDataProvider;
        
        private Dictionary<int, ItemView> _viewsByItemId = new Dictionary<int, ItemView>();

        private bool _isInited;
        
        public event Action<ItemView, int> OnItemSelected;

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
                _viewsByItemId.Add(item.ItemId, itemView);
            }

            _isInited = true;
        }

        public void SetSelectedViewsByItemId(IEnumerable<int> itemIds)
        {
            foreach (var (key, itemView) in _viewsByItemId)
            {
                itemView.SetSelected(itemIds.Contains(key));
            }
        }

        private void ItemView_OnClicked(ItemView itemView, int obj)
        {
            if (selectableViews)
            {
                itemView.SetSelected(!itemView.IsSelected);
            }
            
            OnItemSelected?.Invoke(itemView, obj);
            if(hideOnSelect)
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