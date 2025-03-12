using System;
using Gameplay.Map.Building;
using NUnit.Framework.Internal.Builders;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Buildings.View
{
    public class SelectBuildingView: MonoBehaviour
    {
        [SerializeField] 
        private Button _button;
        [SerializeField]
        private BuildingSettingsDataAsset _buildingSettingsDataAsset;
        
        private IBuildingMapSpawnSelector _buildingMapSpawnSelector;

        public event Action<string> OnBuildingSelected;
        
        private void Start()
        {
            _button.onClick.AddListener(Button_OnClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Button_OnClicked);
        }

        private void Button_OnClicked()
        {
            _buildingMapSpawnSelector.Select(_buildingSettingsDataAsset.Key);
        }
    }
}