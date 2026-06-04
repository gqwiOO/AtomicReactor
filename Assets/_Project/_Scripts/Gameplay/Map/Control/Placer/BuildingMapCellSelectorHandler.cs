using Core.Scripts.Debugging;
using Cysharp.Threading.Tasks;
using Gameplay.Control;
using Gameplay.Control.Keyboard;
using Gameplay.Map.Building.Factory;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Building.Validator;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Placer
{
    public class BuildingMapCellSelectorHandler : MonoBehaviour, IBuildingMapCellSelectorHandler
    {
        [SerializeField] private Vector3 cellDiffVisitorPosition;

        private ICellMapListener _cellMapListener;
        private IBuildingMapSpawnSelector _buildingMapSpawnSelector;
        private IKeyboardManager _keyboardManager;
        private BuildingMapObject _instance;

        private bool _isEnabled;
        private IBuildingMapFactory _buildingMapFactory;
        private IBuildingCellPlacementValidator _buildingCellPlacementValidator;
        private IBuildingsSettingsProvider _buildingsSettingsProvider;
        private bool _canBePlaced = true;
        private BuildingSettingsDataAsset _currentBuildingSettingsData;
        private bool _ignoringCurrentPress;

        [Inject]
        private void Construct(ICellMapListener cellMapListener, IBuildingMapSpawnSelector buildingMapSpawnSelector,
            IBuildingMapFactory buildingMapFactory, IBuildingCellPlacementValidator buildingCellPlacementValidator,
            IBuildingsSettingsProvider buildingsSettingsProvider, IKeyboardManager keyboardManager)
        {
            _buildingsSettingsProvider = buildingsSettingsProvider;
            _buildingCellPlacementValidator = buildingCellPlacementValidator;
            _buildingMapFactory = buildingMapFactory;
            _buildingMapSpawnSelector = buildingMapSpawnSelector;
            _cellMapListener = cellMapListener;
            _keyboardManager = keyboardManager;
        }

        private void Start()
        {
            _buildingMapSpawnSelector.OnBuildingChanged += BuildingMapSpawnSelectorOnOnBuildingChanged;
            _keyboardManager.RegisterAction(GameAction.CancelBuildingPlacement, OnCancelPlacement);
        }

        private void OnDestroy()
        {
            _keyboardManager.UnregisterAction(GameAction.CancelBuildingPlacement, OnCancelPlacement);
        }

        private void BuildingMapSpawnSelectorOnOnBuildingChanged(BuildingSettingsDataAsset buildingSettingsData)
        {
            SetCurrentBuilding(buildingSettingsData);
        }

        private async UniTask SetCurrentBuilding(BuildingSettingsDataAsset buildingSettingsData)
        {
            await UniTask.WaitForSeconds(0.1f, true);
            _currentBuildingSettingsData = buildingSettingsData;
            TurnOn(_currentBuildingSettingsData.Key);
        }

        public void TurnOn(string key)
        {
            ClearPreviousState();

            _isEnabled = true;
            _ignoringCurrentPress = Input.GetMouseButton(0);
            _cellMapListener.OnCellPointed += CellMapListener_OnCellPointed;

            _instance = _buildingMapFactory.CreateBuilding<BuildingMapObject>(key);
            _instance.DisableAllColliders();
        }

        private void ClearPreviousState()
        {
            if (_isEnabled)
            {
                _cellMapListener.OnCellPointed -= CellMapListener_OnCellPointed;
                Destroy(_instance.gameObject);
                _isEnabled = false;
            }
        }

        public void TurnOff()
        {
            _isEnabled = false;
            _cellMapListener.OnCellPointed -= CellMapListener_OnCellPointed;
        }

        private void OnCancelPlacement()
        {
            if (!_isEnabled) return;
            ClearPreviousState();
            _buildingMapSpawnSelector.Unselect();
        }

        private void CellMapListener_OnCellPointed(CellComponent cell)
        {
            _canBePlaced = _buildingCellPlacementValidator.CanBePlaced(
                _buildingsSettingsProvider.GetBuildingSettings(_instance.Key), cell.Position);

            Debugging.Log(this, "Can be placed: " + _canBePlaced);

            _instance.SetMaterialColor(_canBePlaced ? Color.green : Color.red);
        }

        private void Update()
        {
            if (!_isEnabled) return;

            if (_cellMapListener.CurrentCell != null)
                _instance.transform.position = _cellMapListener.CurrentCell.transform.position + cellDiffVisitorPosition;

            if (_ignoringCurrentPress)
            {
                if (!Input.GetMouseButton(0))
                    _ignoringCurrentPress = false;
                return;
            }

            if (Input.GetMouseButton(0) && _canBePlaced)
                PlaceBuilding().Forget();
        }

        private async UniTask PlaceBuilding()
        {
            _cellMapListener.CurrentCell.SetVisitor(_instance);
            TurnOff();
            _instance.SetMaterialColor(Color.white);
            var placedCell = _cellMapListener.CurrentCell;
            _instance.StartBuilding(placedCell.Position);
            _instance = null;
            if (_currentBuildingSettingsData.MultiplyPlacing)
            {
                TurnOn(_currentBuildingSettingsData.Key);
                _canBePlaced = _buildingCellPlacementValidator.CanBePlaced(
                    _buildingsSettingsProvider.GetBuildingSettings(_currentBuildingSettingsData.Key),
                    placedCell.Position);
            }
        }
    }
}
