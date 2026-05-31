using Gameplay.Control.Data;
using Gameplay.Control.Keyboard;
using UnityEngine;
using Zenject;

namespace Gameplay.Control
{
    public class ControlInitializer : MonoBehaviour
    {
        [SerializeField] private KeyboardSettingsAsset keyboardSettingsAsset;

        private IKeyboardManager _keyboardManager;
        private IMapKeyboardControl _mapKeyboardControl;

        [Inject]
        private void Construct(IKeyboardManager keyboardManager, IMapKeyboardControl mapKeyboardControl)
        {
            _keyboardManager = keyboardManager;
            _mapKeyboardControl = mapKeyboardControl;
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _keyboardManager.Init(keyboardSettingsAsset.KeyboardSettings);
            _mapKeyboardControl.Init(keyboardSettingsAsset.KeyboardSettings);
        }
    }
}
