using Gameplay.Map.Building;
using Gameplay.MapUI;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Buildings.Triggers
{
    public class MouseCursorOverTrigger: BaseBuildingMapObjectTrigger
    {
        private BuildingMapObject _buildingMapObject;
        
        private void Awake()
        {
            _buildingMapObject = GetComponent<BuildingMapObject>();
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            Trigger(_buildingMapObject);
        }

        private void OnMouseExit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            Untrigger();
        }
    }
}