using Gameplay.Map.Building;
using Gameplay.MapUI;
using UnityEngine;
using Zenject;

namespace Gameplay.Buildings.Triggers
{
    public class MouseCursorOverUIViewTrigger: MonoBehaviour
    {
        private IMapUIHandler _mapUIHandler;
        private BuildingMapObject _buildingMapObject;

        [Inject]
        private void Construct(IMapUIHandler mapUIHandler)
        {
            _mapUIHandler = mapUIHandler;
        }

        private void Awake()
        {
            _buildingMapObject = GetComponent<BuildingMapObject>();
        }

        private void OnMouseEnter()
        {
            _mapUIHandler.ShowMapBuildingView(_buildingMapObject);
        }

        private void OnMouseExit()
        {
            _mapUIHandler.HideMapBuildingView(_buildingMapObject);
        }
    }
}