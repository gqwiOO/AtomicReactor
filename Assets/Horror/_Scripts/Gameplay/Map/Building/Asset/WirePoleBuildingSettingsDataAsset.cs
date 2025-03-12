using System;
using Gameplay.Map.Building.Furnace;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/WirePoleBuildingSettingsDataAsset", fileName = "WirePoleBuildingSettingsDataAsset")]
    public class WirePoleBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        public override string Key => GeneratorBuildingSettingsData.Key;
        public override string Name => GeneratorBuildingSettingsData.Name;
        [field: SerializeField] 
        public WirePoleBuildingSettingsData GeneratorBuildingSettingsData { get; private set; }
    }

    [Serializable]
    public class WirePoleBuildingSettingsData: BuildingSettingsData
    {
        public int Radius;
    }
}