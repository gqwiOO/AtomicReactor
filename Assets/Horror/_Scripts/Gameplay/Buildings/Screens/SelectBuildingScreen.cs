using System.Collections.Generic;
using Gameplay.Buildings.View;
using Gameplay.Map.Building;
using Raccoons.UI.Screens;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Gameplay.Buildings.Screens
{
    public class SelectBuildingScreen: BaseScreen
    {
        [SerializeField] private BuildingsCollectionView buildingsCollectionView;
        
        private IBuildingMapSpawnSelector _buildingMapSpawnSelector;
        
        [Inject]
        private void Construct(IBuildingMapSpawnSelector buildingMapSpawnSelector)
        {
            _buildingMapSpawnSelector = buildingMapSpawnSelector;
        }
        private void Start()
        {
            buildingsCollectionView.BuildingViews.ForEach(item => item.InteractableItem.OnClicked += SelectBuildingView_OnBuildingSelected);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            buildingsCollectionView.BuildingViews.ForEach(item => item.InteractableItem.OnClicked -= SelectBuildingView_OnBuildingSelected);
        }

        private void SelectBuildingView_OnBuildingSelected(string key)
        {
            Close();
            _buildingMapSpawnSelector.Select(key);
        }
    }
}