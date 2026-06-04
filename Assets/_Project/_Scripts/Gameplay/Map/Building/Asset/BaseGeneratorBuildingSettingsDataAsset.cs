using Gameplay.Map.Building.Generators;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/GeneratorBuildingSettingsDataAsset", fileName = "GeneratorBuildingSettingsDataAsset")]
    public class BaseGeneratorBuildingSettingsDataAsset<TGeneratorSettings> : BuildingSettingsDataAsset where TGeneratorSettings : GeneratorBuildingSettingsData
    {
        [field: SerializeField] 
        public TGeneratorSettings GeneratorBuildingSettingsData { get; private set; }
    }
}