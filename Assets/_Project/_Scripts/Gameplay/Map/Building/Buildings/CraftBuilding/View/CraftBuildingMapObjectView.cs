using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Gameplay.Crafting;
using Gameplay.Map.Building.Items.View;
using Gameplay.MapUI.Views;
using Mechanics.Pools;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.CraftBuilding.View
{
    public class CraftBuildingMapObjectView: BaseMapObjectView
    {
        [SerializeField] private CraftResultView craftResultView;

        [SerializeField] private AllItemsPicker allItemsPicker;

        [Header("Pooling")]
        [SerializeField] private PoolGameObjects pool;
        [SerializeField] private Transform container;
        
        private ICraftService _craftService;
        private CraftBuildingMapObject _craftBuildingMapObject;

        [Inject]
        private void Construct(ICraftService craftService)
        {
            _craftService = craftService;
        }
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            pool.Initialize();
            SpecificInit(buildingMapObject as CraftBuildingMapObject);
        }

        private void SpecificInit(CraftBuildingMapObject craftBuildingMapObject)
        {
            _craftBuildingMapObject = craftBuildingMapObject;
            craftResultView.Init(_craftBuildingMapObject.ItemOutputContainer);
        }

        protected override void Start()
        {
            base.Start();
            craftResultView.OnClicked += ItemView_OnClicked;
            allItemsPicker.OnItemSelected += AllItemsPicker_OnItemSelected;
        }

        private void AllItemsPicker_OnItemSelected(ItemView itemView, int itemId)
        {
            craftResultView.Set(itemId);
            allItemsPicker.Hide();
            InitCraftItemsView(itemId);

            _craftBuildingMapObject.SetRecipe(_craftService.GetItemsCraft(itemId));
        }

        private void InitCraftItemsView(int itemId)
        {
            CraftData craftItems = _craftService.GetItemsCraft(itemId);
            foreach (ICraftItem craftItem in craftItems.Inputs)
            {
                CraftItemView view = GetRequiredItemView();
                view.Init(_craftBuildingMapObject.ItemsInputInventory, craftItem.ItemId, craftItem.Amount);
                view.transform.SetParent(container);
                view.gameObject.SetActive(true);
            }
        }

        private CraftItemView GetRequiredItemView()
        {
            CraftItemView result = pool.Pull().GetOwner<CraftItemView>();
            return result;
        }

        private void ItemView_OnClicked(int obj)
        {
            allItemsPicker.Init();
            allItemsPicker.Show();
        }
    }
}