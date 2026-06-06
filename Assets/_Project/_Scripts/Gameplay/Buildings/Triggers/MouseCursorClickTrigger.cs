using Gameplay.Map.Building;
using Gameplay.MapUI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Buildings.Triggers
{
    public class MouseCursorClickTrigger: MonoBehaviour
    {
        private IMapUIHandler _mapUIHandler;

        [Inject]
        private void Construct(IMapUIHandler mapUIHandler)
        {
            _mapUIHandler = mapUIHandler;
        }
        private void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            _mapUIHandler.ShowMapBuildingView(GetComponent<BuildingMapObject>());
        }
    }
}