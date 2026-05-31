using Gameplay.Map.Building;
using Gameplay.Map.Building.View;
using Gameplay.MapUI.Views;
using UnityEngine;
using Zenject;

namespace Gameplay.MapUI
{
    public class MapUIHandler: MonoBehaviour,IMapUIHandler
    {
        private const string BUILDING_SIDES_SETTINGS_VIEW_ID = "building_sides_settings_view";

        
        private IMapUIProvider _mapUIProvider;

        [Inject]
        private void Construct(IMapUIProvider mapUIProvider)
        {
            _mapUIProvider = mapUIProvider;
        }
        
        public void ShowMapBuildingView(BuildingMapObject electricFurnaceBuilding)
        {
            IMapUIObjectView view = _mapUIProvider.GetMapUIView(electricFurnaceBuilding.Key);
            view.Init(electricFurnaceBuilding);
            view.Show();
        }

        public void HideMapBuildingView(BuildingMapObject electricFurnaceBuilding)
        {
            IMapUIObjectView view = _mapUIProvider.GetMapUIView(electricFurnaceBuilding.Key);            
            view.Hide();
        }
        
        public void HideElectricFurnaceView()
        {
            ElectricFurnaceView view = _mapUIProvider.GetElectricFurnaceView();
            view.Hide();
        }
        
        public void ShowBuildingSidesSettingsView(BuildingMapObject buildingSidesData)
        {
            var view  = _mapUIProvider.GetMapUIView(BUILDING_SIDES_SETTINGS_VIEW_ID) as BuildingSidesView;
            view?.Init(buildingSidesData);
            view?.Show();
        }
    }
}