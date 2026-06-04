using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Tabs
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private bool _hasVisuals;

        [ShowIf(nameof(_hasVisuals))]
        [SerializeField] private GameObject _selectedVisual;

        [ShowIf(nameof(_hasVisuals))]
        [SerializeField] private GameObject _deselectedVisual;

        public event Action<TabButton> OnClicked;

        public bool IsSelected { get; private set; }

        private void Awake()
        {
            _button.onClick.AddListener(Button_OnClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Button_OnClicked);
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;

            if (_hasVisuals)
            {
                _selectedVisual.SetActive(selected);
                _deselectedVisual.SetActive(!selected);
            }
        }

        private void Button_OnClicked()
        {
            OnClicked?.Invoke(this);
        }
    }
}
