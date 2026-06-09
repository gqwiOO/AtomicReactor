using System.Collections.Generic;
using Gameplay.Buildings.View;
using Gameplay.Map.Building;
using Gameplay.Map.Control;
using Raccoons.UI.Screens;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Gameplay.Buildings.Screens
{
    public class SelectBuildingScreen: BaseScreen
    {
        [SerializeField] private List<BuildingsCollectionView> buildingsCollectionView;
        
        private IBuildingMapSpawnSelector _buildingMapSpawnSelector;
        private bool _inited;
        private MapControlService _mapControlService;

        [Inject]
        private void Construct(IBuildingMapSpawnSelector buildingMapSpawnSelector, MapControlService mapControlService)
        {
            _mapControlService = mapControlService;
            _buildingMapSpawnSelector = buildingMapSpawnSelector;
        }
        
        public void Init()
        {
            if (!_inited)
            {
                buildingsCollectionView.ForEach(collectionView => collectionView.OnBuildingToBuildSelected += SelectBuildingView_OnBuildingSelected);
                _inited = true;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            buildingsCollectionView.ForEach(collectionView => collectionView.OnBuildingToBuildSelected -= SelectBuildingView_OnBuildingSelected);
        }

        private void SelectBuildingView_OnBuildingSelected(string key)
        {
            Close();
            _mapControlService.SetBuildingControlMode(key);
            _buildingMapSpawnSelector.Select(key);
        }
    }
}