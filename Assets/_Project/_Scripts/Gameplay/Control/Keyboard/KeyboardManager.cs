using System;
using System.Collections.Generic;
using _Project.Core.Services.UpdateService;
using Gameplay.Control.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Control.Keyboard
{
    public class KeyboardManager : IKeyboardManager, IUpdatable
    {
        private readonly Dictionary<KeyCode, GameAction> _keyToAction = new();
        private readonly Dictionary<GameAction, Action> _listeners = new();
        private IUpdateService _updateService;

        public UpdateType UpdateType => UpdateType.Update;

        [Inject]
        private void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        public void Init(KeyboardSettings settings)
        {
            _keyToAction.Clear();
            foreach (var binding in settings.Bindings)
                _keyToAction[binding.Key] = binding.Action;

            _updateService.Add(this);
        }

        public void RegisterAction(GameAction action, Action callback)
        {
            if (_listeners.ContainsKey(action))
                _listeners[action] += callback;
            else
                _listeners[action] = callback;
        }

        public void UnregisterAction(GameAction action, Action callback)
        {
            if (_listeners.TryGetValue(action, out var existing))
                _listeners[action] = existing - callback;
        }

        public void Tick()
        {
            if (!Input.anyKeyDown) return;
            foreach (var (keyCode, action) in _keyToAction)
            {
                if (Input.GetKeyDown(keyCode) && _listeners.TryGetValue(action, out var handler))
                    handler?.Invoke();
            }
        }
    }
}
