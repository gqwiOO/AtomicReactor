using Gameplay.Map.Building.Generators;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/GeneratorBuildingSettingsDataAsset", fileName = "GeneratorBuildingSettingsDataAsset")]
    public class GeneratorBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        [field: SerializeField] 
        public GeneratorBuildingSettingsData GeneratorBuildingSettingsData { get; private set; }
    }
}