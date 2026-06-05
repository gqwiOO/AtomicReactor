using System;
using System.Linq;
using Gameplay.Map.Building;
using Gameplay.Map.Building.Items.View;
using Gameplay.Transportation.Sorters;
using UnityEngine;

namespace Gameplay.MapUI.Views
{
    public class ItemSorterView: BaseMapObjectView
    {
        [SerializeField] private AllItemsPicker allItemsView;
        private ItemSorter _itemSorter;

        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as ItemSorter);
            allItemsView.Init();
            allItemsView.SetSelectedViewsByItemId(_itemSorter.AllowedItems);
        }

        private void SpecificInit(ItemSorter itemSorter)
        {
            _itemSorter = itemSorter;
        }

        private void OnEnable()
        {
            allItemsView.OnItemSelected += AllItemsView_OnItemSelected;
        }
        
        private void OnDisable()
        {
            allItemsView.OnItemSelected -= AllItemsView_OnItemSelected;
        }

        private void AllItemsView_OnItemSelected(ItemView itemView, int itemId)
        {
            _itemSorter.SetItemAllowedState(itemId, itemView.IsSelected);
        }
    }
}