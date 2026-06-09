using Gameplay.Map.Cell;

namespace Gameplay.Map.Control
{
    public class MapBuildingControlTypeHandler: IMapControlTypeHandler
    {
        public ControlMode ControlMode => ControlMode.Building;
        public void HandleCellClick(ICell buildingMapObject)
        {
            
        }
    }
}