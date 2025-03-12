using _Project.Core.Services.UpdateService;
using Gameplay.Control.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Control.Keyboard
{
    public class KeyboardControl : IKeyboardControl, IUpdatable
    {
        private KeyboardSettings _keyboardSettings;
        private IMapKeyboardControl _mapKeyboardControl;
        private IUpdateService _updateService;
        public UpdateType UpdateType => UpdateType.Update;


        [Inject]
        private void Construct(IMapKeyboardControl mapKeyboardControl, IUpdateService updateService)
        {
            _updateService = updateService;
            _mapKeyboardControl = mapKeyboardControl;
        }

        public void Init(KeyboardSettings keyboardSettings)
        {
            _updateService.Add(this);
            _keyboardSettings = keyboardSettings;
        }

        public void Tick()
        {
            if (Input.anyKeyDown)
            {
                if(Input.GetKeyDown(_keyboardSettings.BuildingSidesSettingsKey))
                    _mapKeyboardControl.HandleInteraction(_keyboardSettings.BuildingSidesSettingsKey);
                    
            }
        }
    }
}