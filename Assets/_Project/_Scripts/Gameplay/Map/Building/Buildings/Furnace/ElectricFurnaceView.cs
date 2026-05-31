using System;
using Gameplay.Buildings.View;
using Gameplay.Map.Building;
using Gameplay.Map.Building.Electricity;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.MapUI.Views
{
    public class ElectricFurnaceView: BaseMapObjectView
    {
        [SerializeField] 
        private ElectricityCurrentValueView _electricityCurrentValueView;
        
        [SerializeField] 
        private ItemContainerCountView itemInputContainerCountView;
        [SerializeField] 
        private ItemContainerCountView itemOutputContainerCountView;

        [SerializeField] 
        private Button _buildingSideSettings;

        private IMapUIHandler _mapUIHandler;
        private ElectricFurnaceMapBuilding _electricTwoItemsMechanism;
        
        [Inject]
        private void Construct(IMapUIHandler mapUIHandler)
        {
            _mapUIHandler = mapUIHandler;
        }

        protected override void Start()
        {
            _buildingSideSettings.onClick.AddListener(BuildingSideSettings_OnClick);
            base.Start();
        }

        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as ElectricFurnaceMapBuilding);
        }

        private void SpecificInit(ElectricFurnaceMapBuilding electricTwoItemsMechanism)
        {
            _electricTwoItemsMechanism = electricTwoItemsMechanism;
            _electricityCurrentValueView.Init(electricTwoItemsMechanism.ElectricTwoItemsMechanismBuildingCoreCore.ElectricityContainer);
            itemInputContainerCountView.Init(electricTwoItemsMechanism.ElectricTwoItemsMechanismBuildingCoreCore.ItemInputContainer);
            itemOutputContainerCountView.Init(electricTwoItemsMechanism.ElectricTwoItemsMechanismBuildingCoreCore.ItemOutputContainer);
        }

        private void BuildingSideSettings_OnClick()
        {
            _mapUIHandler.ShowBuildingSidesSettingsView(_electricTwoItemsMechanism);
        }

        protected override void OnDestroy()
        {
            _buildingSideSettings.onClick.RemoveListener(BuildingSideSettings_OnClick);
            base.OnDestroy();
        }
    }
}