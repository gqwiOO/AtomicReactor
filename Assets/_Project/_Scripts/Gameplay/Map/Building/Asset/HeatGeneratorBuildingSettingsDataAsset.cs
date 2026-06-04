using Gameplay.Map.Building.Generators.HeatGenerator;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/HeatGeneratorBuildingSettingsDataAsset", fileName = "HeatGeneratorBuildingSettingsDataAsset")]
    public class HeatGeneratorBuildingSettingsDataAsset : BaseGeneratorBuildingSettingsDataAsset<HeatGeneratorSettingsData>
    {
        public HeatGeneratorSettingsData HeatGeneratorSettingsData => GeneratorBuildingSettingsData as HeatGeneratorSettingsData; 
    }
}
