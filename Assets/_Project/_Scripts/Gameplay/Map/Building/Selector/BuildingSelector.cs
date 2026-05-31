using System;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Selector
{
    public class BuildingSelector : MonoBehaviour, IBuildingSelector
    {
        [SerializeField] private Camera mainCamera;

        private IBuildingMapSpawnSelector _buildingMapSpawnSelector;

        public BuildingMapObject SelectedBuilding { get; private set; }
        public bool IsSelectionLocked { get; set; }

        public event Action<BuildingMapObject> OnSelected;
        public event Action OnDeselected;

        [Inject]
        private void Construct(IBuildingMapSpawnSelector buildingMapSpawnSelector)
        {
            _buildingMapSpawnSelector = buildingMapSpawnSelector;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (IsSelectionLocked) return;
            if (_buildingMapSpawnSelector.CurrentBuilding != null) return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var building = hit.collider.GetComponentInParent<BuildingMapObject>();
                if (building == null)
                {
                    var cell = hit.collider.GetComponent<CellComponent>();
                    if (cell?.CellVisitor is BuildingMapObject cellBuilding)
                        building = cellBuilding;
                }

                if (building != null)
                {
                    Select(building);
                    return;
                }
            }

            Deselect();
        }

        private void Select(BuildingMapObject building)
        {
            if (SelectedBuilding == building) return;
            Deselect();

            SelectedBuilding = building;
            building.ShowSelectionOutline();
            OnSelected?.Invoke(building);
        }

        public void Deselect()
        {
            if (SelectedBuilding == null) return;

            SelectedBuilding.HideSelectionOutline();
            SelectedBuilding = null;
            OnDeselected?.Invoke();
        }
    }
}
