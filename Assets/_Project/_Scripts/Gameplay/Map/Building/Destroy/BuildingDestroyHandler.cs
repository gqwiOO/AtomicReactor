using Cysharp.Threading.Tasks;
using Gameplay.Buildings.Screens;
using Gameplay.Control;
using Gameplay.Control.Keyboard;
using Gameplay.Map.Building.Chest;
using Gameplay.Map.Building.Selector;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.CellsService;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Destroy
{
    public class BuildingDestroyHandler : MonoBehaviour
    {
        [SerializeField] private ConfirmDestroyBuildingPopup popup;

        private IBuildingSelector _buildingSelector;
        private IKeyboardManager _keyboardManager;
        private IMapCellsService _mapCellsService;
        private IBuildingsSettingsProvider _buildingsSettingsProvider;
        private bool _isPopupShowing;

        [Inject]
        private void Construct(IBuildingSelector buildingSelector, IKeyboardManager keyboardManager,
            IMapCellsService mapCellsService, IBuildingsSettingsProvider buildingsSettingsProvider)
        {
            _buildingSelector = buildingSelector;
            _keyboardManager = keyboardManager;
            _mapCellsService = mapCellsService;
            _buildingsSettingsProvider = buildingsSettingsProvider;
        }

        private void Start()
        {
            _keyboardManager.RegisterAction(GameAction.DestroyBuilding, OnDestroyAction);
        }

        private void OnDestroy()
        {
            _keyboardManager.UnregisterAction(GameAction.DestroyBuilding, OnDestroyAction);
        }

        private void OnDestroyAction()
        {
            if (_buildingSelector.SelectedBuilding == null) return;
            if (_isPopupShowing) return;
            HandleDestroy().Forget();
        }

        private async UniTask HandleDestroy()
        {
            _isPopupShowing = true;
            _buildingSelector.IsSelectionLocked = true;

            bool confirmed = await popup.ShowAndAwaitResult();

            _isPopupShowing = false;
            _buildingSelector.IsSelectionLocked = false;

            if (!confirmed) return;

            var building = _buildingSelector.SelectedBuilding;
            if (building == null) return;

            RefundResources(building);

            var cell = _mapCellsService.GetCell(building.CellPosition);
            _buildingSelector.Deselect();
            cell?.SetVisitor(null);
            Destroy(building.gameObject);
        }

        private void RefundResources(BuildingMapObject building)
        {
            var settings = _buildingsSettingsProvider.GetBuildingSettings(building.Key);
            if (settings?.BuildingCost == null || settings.BuildingCost.Count == 0) return;

            _mapCellsService.GetClosestCellWithVisitor<ChestBuilding>(out var chest, building.CellPosition);
            if (chest == null) return;

            foreach (var cost in settings.BuildingCost)
            {
                int refund = Mathf.FloorToInt(cost.Amount * 0.5f);
                if (refund > 0)
                    chest.AddResource(cost.Item.ItemId, refund);
            }
        }
    }
}
