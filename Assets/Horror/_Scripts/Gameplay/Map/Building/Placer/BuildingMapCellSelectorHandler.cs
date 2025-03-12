using System;
using Gameplay.Control;
using Gameplay.Map.Building.Factory;
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

        [Inject]
        private void Construct(ICellMapListener cellMapListener, IBuildingMapSpawnSelector buildingMapSpawnSelector,
            IBuildingMapFactory buildingMapFactory)
        {
            _buildingMapFactory = buildingMapFactory;
            _buildingMapSpawnSelector = buildingMapSpawnSelector;
            _cellMapListener = cellMapListener;
        }
        
        private void Start()
        {
            _buildingMapSpawnSelector.OnBuildingChanged += BuildingMapSpawnSelectorOnOnBuildingChanged;
        }

        private void BuildingMapSpawnSelectorOnOnBuildingChanged(BuildingSettingsDataAsset obj)
        {
            TurnOn(obj.Key);
        }

        public void TurnOn(string key)
        {
            _isEnabled = true;
            _cellMapListener.OnCellPointed += CellMapListener_OnCellPointed;

            _instance = _buildingMapFactory.CreateBuilding<BuildingMapObject>(key);
            _instance.DisableTriggerCollider();
        }

        public void TurnOff()
        {
            _isEnabled = false;
            _cellMapListener.OnCellPointed -= CellMapListener_OnCellPointed;
        }

        private void CellMapListener_OnCellPointed(CellComponent cell)
        {
            _instance.transform.position = _cellMapListener.CurrentCell.transform.position + cellDiffVisitorPosition;
        }

        private void Update()
        {
            if (!_isEnabled)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                _cellMapListener.CurrentCell.SetVisitor(_instance);
                TurnOff();
                _instance.StartBuilding(_cellMapListener.CurrentCell.Position);
                _instance = null;
            }
        }
    }
}