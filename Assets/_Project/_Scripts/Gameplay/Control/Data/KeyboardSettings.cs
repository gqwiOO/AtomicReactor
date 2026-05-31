using System;
using System.Collections.Generic;
using Gameplay.Control.Keyboard;
using UnityEngine;

namespace Gameplay.Control.Data
{
    [Serializable]
    public class KeyBinding
    {
        public GameAction Action;
        public KeyCode Key;
    }

    [Serializable]
    public class KeyboardSettings
    {
        [SerializeField] private List<KeyBinding> _bindings = new();

        public IReadOnlyList<KeyBinding> Bindings => _bindings;
    }
}
