using System;
using Gameplay.Control.Data;
using Gameplay.Control.Keyboard;

namespace Gameplay.Control
{
    public interface IKeyboardManager
    {
        void Init(KeyboardSettings settings);
        void RegisterAction(GameAction action, Action callback);
        void UnregisterAction(GameAction action, Action callback);
    }
}
