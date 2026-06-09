using System;
using Core.Scripts.Debugging;
using Cysharp.Threading.Tasks;
using Gameplay.Control;
using Gameplay.Control.Keyboard;
using Gameplay.Map.Building;
using Gameplay.Map.Building.Factory;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Building.Validator;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Gameplay.Map.Control
{
    public class MapBuildingControlTypeHandler : IMapControlTypeHandler
    {
        private ICellMapListener _cellMapListener;
        private IBuildingMapSpawnSelector _buildingMapSpawnSelector;
        private IKeyboardManager _keyboardManager;
        private IBuildingMapFactory _buildingMapFactory;
        private IBuildingCellPlacementValidator _buildingCellPlacementValidator;
        private IBuildingsSettingsProvider _buildingsSettingsProvider;

        private BuildingMapObject _instance;
        private BuildingSettingsDataAsset _currentBuildingSettingsData;
        private bool _canBePlaced = true;
        private ControlArgs _controlArgs;

        public ControlMode ControlMode => ControlMode.Building;

        public event Action OnSelfChangeModeToDefault;

        public bool State { get; set; }

        [Inject]
        public MapBuildingControlTypeHandler(ICellMapListener cellMapListener, IBuildingMapSpawnSelector buildingMapSpawnSelector,
            IBuildingMapFactory buildingMapFactory, IBuildingCellPlacementValidator buildingCellPlacementValidator,
            IBuildingsSettingsProvider buildingsSettingsProvider, IKeyboardManager keyboardManager)
        {
            _buildingsSettingsProvider = buildingsSettingsProvider;
            _buildingCellPlacementValidator = buildingCellPlacementValidator;
            _buildingMapFactory = buildingMapFactory;
            _buildingMapSpawnSelector = buildingMapSpawnSelector;
            _cellMapListener = cellMapListener;
            _keyboardManager = keyboardManager;
            
            _buildingMapSpawnSelector.OnBuildingChanged += OnBuildingChanged;
            _keyboardManager.RegisterAction(GameAction.CancelBuildingPlacement, OnCancelPlacement);
        }
        
        // private void OnDestroy()
        // {
        //     _keyboardManager.UnregisterAction(GameAction.CancelBuildingPlacement, OnCancelPlacement);
        // }

        public void Init(ControlArgs controlArgs)
        {
            _controlArgs = controlArgs;
        }

        public void SetState(bool state)
        {
            State = state;
            if (state)
            {
                _cellMapListener.OnCellPointed += OnCellPointed;

                _instance = _buildingMapFactory.CreateBuilding<BuildingMapObject>(_controlArgs.BuildingKey);
                _instance.DisableAllColliders();
            }
            else
            {
                _cellMapListener.OnCellPointed -= OnCellPointed;
            }
        }

        public void HandleCellChanged(ICell cell)
        {
            _instance.transform.position = _cellMapListener.CurrentCell.transform.position + new Vector3(0, 1, 0)/*+ cellDiffVisitorPosition*/;
        }

        public void HandleCellClick(ICell cell)
        {
            if (_canBePlaced)
                PlaceBuilding().Forget();
        }

        private void OnBuildingChanged(BuildingSettingsDataAsset buildingSettingsData)
        {
            SetCurrentBuilding(buildingSettingsData).Forget();
        }

        private async UniTask SetCurrentBuilding(BuildingSettingsDataAsset buildingSettingsData)
        {
            await UniTask.WaitForSeconds(0.1f, true);
            _currentBuildingSettingsData = buildingSettingsData;
        }
        
        public void TurnOff()
        {
            State = false;
            _cellMapListener.OnCellPointed -= OnCellPointed;
        }

        private void ClearPreviousState()
        {
            if (State)
            {
                _cellMapListener.OnCellPointed -= OnCellPointed;
                Object.Destroy(_instance?.gameObject);
            }
        }

        private void OnCancelPlacement()
        {
            if (!State) return;
            ClearPreviousState();
            _buildingMapSpawnSelector.Unselect();
        }

        private void OnCellPointed(CellComponent cell)
        {
            _canBePlaced = _buildingCellPlacementValidator.CanBePlaced(
                _buildingsSettingsProvider.GetBuildingSettings(_instance.Key), cell.Position);

            Debugging.Log(this, "Can be placed: " + _canBePlaced);

            _instance.SetMaterialColor(_canBePlaced ? Color.green : Color.red);
        }

        private async UniTask PlaceBuilding()
        {
            _cellMapListener.CurrentCell.SetVisitor(_instance);
            TurnOff();
            _instance.SetMaterialColor(Color.white);
            var placedCell = _cellMapListener.CurrentCell;
            _instance.StartBuilding(placedCell.Position);
            _instance = null;
            OnSelfChangeModeToDefault?.Invoke();
            return;
            if (_currentBuildingSettingsData.MultiplyPlacing)
            {
                _canBePlaced = _buildingCellPlacementValidator.CanBePlaced(
                    _buildingsSettingsProvider.GetBuildingSettings(_currentBuildingSettingsData.Key),
                    placedCell.Position);
            }
            else
            {
            }
        }
    }
}
