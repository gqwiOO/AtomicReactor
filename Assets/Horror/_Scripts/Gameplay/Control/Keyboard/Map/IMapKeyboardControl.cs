using Gameplay.Control.Data;
using UnityEngine;

namespace Gameplay.Control.Keyboard
{
    public interface IMapKeyboardControl
    {
        void HandleInteraction(KeyCode keyCode);
        void Init(KeyboardSettings keyboardSettings);
    }
}