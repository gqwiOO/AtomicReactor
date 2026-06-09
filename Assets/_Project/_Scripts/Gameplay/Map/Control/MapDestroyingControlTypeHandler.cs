using Gameplay.Map.Cell;

namespace Gameplay.Map.Control
{
    public class MapDestroyingControlTypeHandler: IMapControlTypeHandler
    {
        public ControlMode ControlMode => ControlMode.Destroying;
        public void HandleCellClick(ICell buildingMapObject)
        {
            
        }
    }
}