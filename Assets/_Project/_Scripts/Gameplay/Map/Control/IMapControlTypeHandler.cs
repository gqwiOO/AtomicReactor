using System;
using Gameplay.Map.Cell;

namespace Gameplay.Map.Control
{
    public interface IMapControlTypeHandler
    {
        ControlMode ControlMode { get; }
        bool State { get; set; }
        event Action OnSelfChangeModeToDefault;
        void Init(ControlArgs controlArgs);
        void SetState(bool state) => State = state;
        void HandleCellClick(ICell cell);
        void HandleCellChanged(ICell cell);
        void HandleCellRelease(ICell cell) { }
    }
}