using System;
using Gameplay.Map.Building.Provider;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Map.Building.View
{
    public class SideView: MonoBehaviour
    {
        [SerializeField] 
        private Image sideIcon;
        
        [SerializeField] 
        private Button button;
        
        private ISidesSettingsProvider _sidesSettingsProvider;
        private SideType _sideType;

        public event Action<SideType> OnSideSettingsChanged;

        [Inject]
        private void Construct(ISidesSettingsProvider sidesSettingsProvider)
        {
            _sidesSettingsProvider = sidesSettingsProvider;
        }

        public void Init(SideType sideType)
        {
            _sideType = sideType;
            Sprite sprite = _sidesSettingsProvider.GetSideSprite(sideType);
            sideIcon.sprite = sprite;
        }

        private void Start() => button.onClick.AddListener(Button_OnClicked);

        private void OnDestroy() => button.onClick.RemoveListener(Button_OnClicked);

        private void Button_OnClicked() => OnSideSettingsChanged?.Invoke(_sideType);
    }
}