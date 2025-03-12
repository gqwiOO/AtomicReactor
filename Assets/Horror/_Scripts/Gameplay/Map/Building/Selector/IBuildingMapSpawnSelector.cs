using System;

namespace Gameplay.Map.Building
{
    public interface IBuildingMapSpawnSelector
    {
        event Action<BuildingSettingsDataAsset> OnBuildingChanged;
        BuildingSettingsDataAsset CurrentBuilding { get; }
        void Select(string key);
        void Unselect();
    }
}