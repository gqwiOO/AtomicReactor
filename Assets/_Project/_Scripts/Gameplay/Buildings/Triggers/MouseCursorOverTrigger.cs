using Gameplay.Map.Building;
using Gameplay.MapUI;
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
            Trigger(_buildingMapObject);
        }

        private void OnMouseExit()
        {
            Untrigger();
        }
    }
}