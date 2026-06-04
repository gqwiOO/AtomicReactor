using Gameplay.Map.Building.Generators.HeatGenerator;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/SteamGeneratorBuildingSettingsDataAsset", fileName = "SteamGeneratorBuildingSettingsDataAsset")]
    public class SteamGeneratorBuildingSettingsDataAsset : BaseGeneratorBuildingSettingsDataAsset<SteamGeneratorSettingsData>
    {
        public SteamGeneratorSettingsData SteamGeneratorSettingsData => GeneratorBuildingSettingsData as SteamGeneratorSettingsData; 
    }
}