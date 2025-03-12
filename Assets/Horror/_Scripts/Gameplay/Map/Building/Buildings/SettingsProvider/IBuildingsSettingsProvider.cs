using Gameplay.Map.Building.Furnace;

namespace Gameplay.Map.Building.SettingsProvider
{
    public interface IBuildingsSettingsProvider
    {
        BuildingSettingsDataAsset GetBuildingSettings(string key);
    }
}