using System;
using Gameplay.Map.Building.Furnace;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/WirePoleBuildingSettingsDataAsset", fileName = "WirePoleBuildingSettingsDataAsset")]
    public class WirePoleBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        [field: SerializeField] 
        public WirePoleBuildingSettingsData GeneratorBuildingSettingsData { get; private set; }
    }

    [Serializable]
    public class WirePoleBuildingSettingsData: BuildingSettingsData
    {
        public int Radius;
    }
}