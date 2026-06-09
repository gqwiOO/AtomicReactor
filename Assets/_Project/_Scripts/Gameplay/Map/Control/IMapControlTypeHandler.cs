using Gameplay.Map.Cell;

namespace Gameplay.Map.Control
{
    public interface IMapControlTypeHandler
    {
        ControlMode ControlMode { get; }
        void HandleCellClick(ICell buildingMapObject);
    }
}