using UnityEngine;

namespace Gameplay.Map.Building.Furnace
{
    [CreateAssetMenu(menuName = "Core/Buildings/FurnaceBuildingSettingsDataAsset", fileName = "FurnaceBuildingSettingsDataAsset")]
    public class TwoItemsMechanismAsset : BuildingSettingsDataAsset
    {
        [field: SerializeField] public TwoItemsMechanismSettingsData TwoItemsMechanismSettingsData { get; private set; }

    }
}