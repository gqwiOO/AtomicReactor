using System;
using Gameplay.Map.Cell;

namespace Gameplay.Map.Control
{
    public class MapDestroyingControlTypeHandler: IMapControlTypeHandler
    {
        public ControlMode ControlMode => ControlMode.Destroying;
        public bool State { get; set; }
        public event Action OnSelfChangeModeToDefault;

        public void Init(ControlArgs controlArgs)
        {
            
        }

        public void HandleCellClick(ICell cell)
        {
            cell.DestroyVisitor();
        }

        public void HandleCellChanged(ICell cell)
        {
            
        }
    }
}