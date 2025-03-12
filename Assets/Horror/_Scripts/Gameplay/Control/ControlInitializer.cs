using System;
using Gameplay.Control.Data;
using Gameplay.Control.Keyboard;
using UnityEngine;
using Zenject;

namespace Gameplay.Control
{
    public class ControlInitializer: MonoBehaviour
    {
        [SerializeField] 
        private KeyboardSettingsAsset keyboardSettingsAsset;
        
        
        private IMapKeyboardControl _mapKeyboardControl;
        private IKeyboardControl _keyboardControl;

        [Inject]
        private void Construct(IMapKeyboardControl mapKeyboardControl, IKeyboardControl keyboardControl)
        {
            _keyboardControl = keyboardControl;
            _mapKeyboardControl = mapKeyboardControl;
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _keyboardControl.Init(keyboardSettingsAsset.KeyboardSettings);
            _mapKeyboardControl.Init(keyboardSettingsAsset.KeyboardSettings);
        }
    }
}