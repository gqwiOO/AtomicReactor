using System;
using Cysharp.Threading.Tasks;
using Gameplay.Control;
using Gameplay.Map.Building;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Building.Validator;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Control
{
    public class MapMovingControlTypeHandler : IMapControlTypeHandler
    {
        private readonly ICellMapListener _cellMapListener;
        private readonly IBuildingCellPlacementValidator _buildingCellPlacementValidator;
        private readonly IBuildingsSettingsProvider _buildingsSettingsProvider;

        private BuildingMapObject _draggedBuilding;
        private ICell _sourceCell;
        private bool _isDragging;
        private bool _canBePlaced;

        public ControlMode ControlMode => ControlMode.Moving;
        public bool State { get; set; }
        public event Action OnSelfChangeModeToDefault;

        [Inject]
        public MapMovingControlTypeHandler(ICellMapListener cellMapListener,
            IBuildingCellPlacementValidator buildingCellPlacementValidator,
            IBuildingsSettingsProvider buildingsSettingsProvider)
        {
            _cellMapListener = cellMapListener;
            _buildingCellPlacementValidator = buildingCellPlacementValidator;
            _buildingsSettingsProvider = buildingsSettingsProvider;
        }

        public void Init(ControlArgs controlArgs) { }

        public void SetState(bool state)
        {
            State = state;
            if (!state)
                CancelDrag();
        }

        public void HandleCellClick(ICell cell)
        {
            if (!State || _isDragging || cell?.CellVisitor is not BuildingMapObject building) return;

            _sourceCell = cell;
            _draggedBuilding = building;
            cell.SetVisitor(null);
            _draggedBuilding.DisableAllColliders();
            _isDragging = true;
            _canBePlaced = false;
            _draggedBuilding.SetMaterialColor(Color.red);
        }

        public void HandleCellChanged(ICell cell)
        {
            if (!_isDragging || cell == null) return;

            _draggedBuilding.transform.position = cell.WorldPosition + 1f * Vector3.up;
            _canBePlaced = _buildingCellPlacementValidator.CanBePlaced(
                _buildingsSettingsProvider.GetBuildingSettings(_draggedBuilding.Key), cell.Position);
            _draggedBuilding.SetMaterialColor(_canBePlaced ? Color.green : Color.red);
        }

        public void HandleCellRelease(ICell cell)
        {
            if (!_isDragging) return;

            if (_canBePlaced && cell != null)
            {
                Place(cell);
            }
            else
            {
                CancelDrag();
            }
        }

        private void Place(ICell targetCell)
        {
            //todo: 1f
            _draggedBuilding.transform.position = targetCell.WorldPosition+ 1f * Vector3.up;
            targetCell.SetVisitor(_draggedBuilding);
            // NOTE: Init re-registers with UpdateService — deduplicate there if needed
            _draggedBuilding.Init(targetCell.Position).Forget();
            _draggedBuilding.SetMaterialColor(Color.white);
            _isDragging = false;
            _draggedBuilding = null;
            _sourceCell = null;
            OnSelfChangeModeToDefault?.Invoke();
        }

        private void CancelDrag()
        {
            if (!_isDragging) return;

            _sourceCell.SetVisitor(_draggedBuilding);
            _draggedBuilding.transform.position = _sourceCell.WorldPosition + 1f * Vector3.up;
            _draggedBuilding.Init(_sourceCell.Position).Forget();
            _draggedBuilding.SetMaterialColor(Color.white);
            _isDragging = false;
            _draggedBuilding = null;
            _sourceCell = null;
            OnSelfChangeModeToDefault?.Invoke();
        }
    }
}
