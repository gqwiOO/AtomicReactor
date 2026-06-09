using Gameplay.Map.Cell;

namespace Gameplay.Map.Control
{
    public class MapDefaultControlTypeHandler: IMapControlTypeHandler
    {
        public ControlMode ControlMode => ControlMode.Default;
        public void HandleCellClick(ICell buildingMapObject)
        {
            
        }
    }
}