using System;
using Sirenix.Utilities;

namespace Gameplay.Control.Mouse
{
    public interface IMouseControlService
    {
        bool IsMouseOverUI { get; }
    }
}