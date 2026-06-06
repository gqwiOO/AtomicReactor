using Gameplay.MapUI.Views;
using Gameplay.Transportation.ItemPipeSystem;
using Gameplay.UI.Views;
using UnityEngine;

namespace Gameplay.Map.Building.Boiler
{
    public class BoilerView : BaseMapObjectView
    {
        [SerializeField] private FloatContainerView waterContainerView;
        [SerializeField] private FloatContainerView steamContainerView;
        [SerializeField] private FuelContainerView fuelContainerView;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as BoilerBuilding);
        }

        private void SpecificInit(BoilerBuilding boiler)
        {
            steamContainerView.Init(boiler.SteamContainer.FloatContainer);
            waterContainerView.Init(boiler.WaterContainer);
            fuelContainerView.Init(boiler.FuelContainer);
        }
    }
}