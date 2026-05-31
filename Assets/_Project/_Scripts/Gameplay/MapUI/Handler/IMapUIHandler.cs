using Gameplay.Map.Building;

namespace Gameplay.MapUI
{
    public interface IMapUIHandler
    {
        void ShowMapBuildingView(BuildingMapObject electricFurnaceBuilding);
        void HideMapBuildingView(BuildingMapObject electricFurnaceBuilding);
        void HideElectricFurnaceView();
        void ShowBuildingSidesSettingsView(BuildingMapObject buildingSidesData);
    }
}