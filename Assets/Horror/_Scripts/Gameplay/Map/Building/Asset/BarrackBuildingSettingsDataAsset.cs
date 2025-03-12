using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/BarrackBuildingSettingsDataAsset", fileName = "BarrackBuildingSettingsDataAsset")]
    public class BarrackBuildingSettingsDataAsset : BuildingSettingsDataAsset
    {
        [SerializeField] private string name;
        public override string Key => BuildingMapObject.Key;
        
        public override string Name => name;
    }
}