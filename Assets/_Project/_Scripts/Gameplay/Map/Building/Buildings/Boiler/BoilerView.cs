using Gameplay.MapUI.Views;
using Gameplay.Transportation.ItemPipeSystem;
using Gameplay.UI.Views;
using UnityEngine;

namespace Gameplay.Map.Building.Boiler
{
    public class BoilerView : BaseMapObjectView
    {
        [SerializeField] private FluidContainerView waterContainerView;
        [SerializeField] private FluidContainerView steamContainerView;
        [SerializeField] private FuelContainerView fuelContainerView;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as BoilerBuilding);
        }

        private void SpecificInit(BoilerBuilding boiler)
        {
            steamContainerView.Init(boiler.SteamContainer);
            waterContainerView.Init(boiler.InsertionFluidContainer);
            fuelContainerView.Init(boiler.FuelContainer);
        }
    }
}