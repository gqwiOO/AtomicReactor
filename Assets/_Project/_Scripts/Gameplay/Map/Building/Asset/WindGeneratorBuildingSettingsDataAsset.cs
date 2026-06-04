using Gameplay.Map.Building.Generators.HeatGenerator;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/WindGeneratorBuildingSettingsDataAsset", fileName = "WindGeneratorBuildingSettingsDataAsset")]
    public class WindGeneratorBuildingSettingsDataAsset : BaseGeneratorBuildingSettingsDataAsset<WindGeneratorSettingsData>
    {
        public WindGeneratorSettingsData WindGeneratorSettingsData => GeneratorBuildingSettingsData as WindGeneratorSettingsData;
    }
}