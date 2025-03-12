using System;
using Gameplay.Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public abstract class BaseInteractableItem: MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private string _key;
        
        public event Action<string> OnClicked;

        public void Init(IKeyData keyData)
        {
            _key = keyData.Key;
        }
        
        public void Init(string key)
        {
            _key = key;
        }
        
        private void Start()
        {
            button.onClick.AddListener(Button_OnClicked);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(Button_OnClicked);
        }

        private void Button_OnClicked()
        {
            OnClicked?.Invoke(_key);
        }
    }
}