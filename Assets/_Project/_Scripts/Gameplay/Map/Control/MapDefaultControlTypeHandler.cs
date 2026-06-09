using System;
using Gameplay.Map.Cell;

namespace Gameplay.Map.Control
{
    public class MapDefaultControlTypeHandler: IMapControlTypeHandler
    {
        public ControlMode ControlMode => ControlMode.Default;
        public bool State { get; set; }
        public event Action OnSelfChangeModeToDefault;

        public void Init(ControlArgs controlArgs)
        {
        }

        public void HandleCellClick(ICell cell)
        {
            
        }

        public void HandleCellChanged(ICell cell)
        {
            
        }
        
    }
}