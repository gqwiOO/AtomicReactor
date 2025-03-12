using Gameplay.Map.Building.Generators;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/GeneratorBuildingSettingsDataAsset", fileName = "GeneratorBuildingSettingsDataAsset")]
    public class GeneratorBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        public override string Key => GeneratorBuildingSettingsData.Key;
        public override string Name => GeneratorBuildingSettingsData.Name;
        [field: SerializeField] 
        public GeneratorBuildingSettingsData GeneratorBuildingSettingsData { get; private set; }
    }
}