using UnityEngine;

namespace Gameplay.Map.Building.CraftBuilding.Data
{
    
    [CreateAssetMenu(menuName = "Core/Buildings/CraftBuildingSettingsDataAsset", fileName = "CraftBuildingSettingsDataAsset")]
    public class CraftBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        [field: SerializeField] public CraftBuildingData CraftBuildingData { get; private set; }
    }
}