using Gameplay.Map.Building;
using Gameplay.MapUI;
using UnityEngine;
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
            _mapUIHandler.ShowMapBuildingView(GetComponent<BuildingMapObject>());
        }
    }
}