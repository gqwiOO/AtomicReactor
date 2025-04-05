using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/ChestBuildingSettingsDataAsset", fileName = "ChestBuildingSettingsDataAsset")]
    public class ChestBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        [field: SerializeField] public InventoryContainerSettingsData ChestSettings { get; private set; }
    }
}