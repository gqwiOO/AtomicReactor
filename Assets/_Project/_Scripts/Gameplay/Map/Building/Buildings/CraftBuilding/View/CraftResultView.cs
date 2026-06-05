using System;
using Gameplay.Inventories;
using Gameplay.Map.Building.Items.View;
using UnityEngine;

namespace Gameplay.Map.Building.CraftBuilding.View
{
    public class CraftResultView: MonoBehaviour
    {
        [SerializeField] private ItemView itemView;
        [SerializeField] private ItemContainerCountView itemContainerValueView;

        public event Action<int> OnClicked;

        private void Start()
        {
            itemView.OnClicked += ItemView_OnClicked;
        }

        private void OnDestroy()
        {
            itemView.OnClicked -= ItemView_OnClicked;
        }

        private void ItemView_OnClicked(ItemView view, int itemId)
        {
            OnClicked?.Invoke(itemId);
        }

        public void Init(SingleCellInventory itemContainer)
        {
            itemContainerValueView.Init(itemContainer);
        }

        public void Set(int itemId)
        {
            itemView.Set(itemId);
        }
    }
}