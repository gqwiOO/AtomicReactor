using UnityEngine;

namespace Gameplay.Map.Building.Mine
{
    [CreateAssetMenu(menuName = "Core/Buildings/MineBuildingSettingsDataAsset", fileName = "MineBuildingSettingsDataAsset")]
    public class MineBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        [field: SerializeField] public MineBuildingSettingsData MineSettings { get; private set; }
    }
}
