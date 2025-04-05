using System;
using System.Threading.Tasks;
using Core.Scripts.Debugging;
using Core.Scripts.Debugging.Logging;
using Cysharp.Threading.Tasks;
using Gameplay.Control;
using Gameplay.Map.Building.Factory;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Building.Validator;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Placer
{
    public class BuildingMapCellSelectorHandler: MonoBehaviour, IBuildingMapCellSelectorHandler
    {
        [SerializeField] private Vector3 cellDiffVisitorPosition;
        
        
        private ICellMapListener _cellMapListener;
        private IBuildingMapSpawnSelector _buildingMapSpawnSelector;
        private BuildingMapObject _instance;


        private bool _isEnabled;
        private IBuildingMapFactory _buildingMapFactory;
        private IBuildingCellPlacementValidator _buildingCellPlacementValidator;
        private IBuildingsSettingsProvider _buildingsSettingsProvider;
        private bool _canBePlaced = true;
        private BuildingSettingsDataAsset _currentBuildingSettingsData;

        [Inject]
        private void Construct(ICellMapListener cellMapListener, IBuildingMapSpawnSelector buildingMapSpawnSelector,
            IBuildingMapFactory buildingMapFactory, IBuildingCellPlacementValidator buildingCellPlacementValidator,
            IBuildingsSettingsProvider buildingsSettingsProvider)
        {
            _buildingsSettingsProvider = buildingsSettingsProvider;
            _buildingCellPlacementValidator = buildingCellPlacementValidator;
            _buildingMapFactory = buildingMapFactory;
            _buildingMapSpawnSelector = buildingMapSpawnSelector;
            _cellMapListener = cellMapListener;
        }
        
        private void Start()
        {
            _buildingMapSpawnSelector.OnBuildingChanged += BuildingMapSpawnSelectorOnOnBuildingChanged;
        }

        private void BuildingMapSpawnSelectorOnOnBuildingChanged(BuildingSettingsDataAsset _currentBuildingSettingsData)
        {
            this._currentBuildingSettingsData = _currentBuildingSettingsData;
            TurnOn(_currentBuildingSettingsData.Key);
        }

        public void TurnOn(string key)
        {
            ClearPreviousState();
            
            _isEnabled = true;
            _cellMapListener.OnCellPointed += CellMapListener_OnCellPointed;

            _instance = _buildingMapFactory.CreateBuilding<BuildingMapObject>(key);
            _instance.DisableTriggerCollider();
        }

        private void ClearPreviousState()
        {
            if (_isEnabled)
            {
                _cellMapListener.OnCellPointed -= CellMapListener_OnCellPointed;
                Destroy(_instance);
                _isEnabled = false;
            }
        }

        public void TurnOff()
        {
            _isEnabled = false;
            _cellMapListener.OnCellPointed -= CellMapListener_OnCellPointed;
        }

        private void CellMapListener_OnCellPointed(CellComponent cell)
        {
            _canBePlaced =  _buildingCellPlacementValidator.CanBePlaced(_buildingsSettingsProvider.GetBuildingSettings(_instance.Key),
                cell.Position);
            
            Debugging.Log(this,"Can be placed: " + _canBePlaced);
            
            if(_canBePlaced)
                _instance.SetMaterialColor(Color.green);
            else
                _instance.SetMaterialColor(Color.red);
            _instance.transform.position = _cellMapListener.CurrentCell.transform.position + cellDiffVisitorPosition;
        }

        private void Update()
        {
            if (!_isEnabled)
                return;
            
            if (Input.GetMouseButton(0) && _canBePlaced)
            {
                PlaceBuilding();
            }
        }

        private async UniTask PlaceBuilding()
        {
            _cellMapListener.CurrentCell.SetVisitor(_instance);
            TurnOff();
            _instance.SetMaterialColor(Color.white);
            _instance.StartBuilding(_cellMapListener.CurrentCell.Position);
            _instance = null;
            if (_currentBuildingSettingsData.MultiplyPlacing)
            {
                TurnOn(_currentBuildingSettingsData.Key);
                _canBePlaced =  _buildingCellPlacementValidator.CanBePlaced(_buildingsSettingsProvider.GetBuildingSettings(_instance.Key),
                    _cellMapListener.CurrentCell.Position);
            }
        }
    }
}