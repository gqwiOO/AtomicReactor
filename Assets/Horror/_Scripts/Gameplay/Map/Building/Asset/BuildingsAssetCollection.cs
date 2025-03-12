using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/BuildingsAssetCollection", fileName = "BuildingsAssetCollection")]
    public class BuildingsAssetCollection: ScriptableObject
    {
        [field: SerializeField] public List<BuildingSettingsDataAsset> BuildingSettingsDataAssets { get; private set; }

        public BuildingSettingsDataAsset GetByKey(string key)
        {
            return BuildingSettingsDataAssets.FirstOrDefault(i => i.Key == key);
        }
        public T GetByKey<T>(string key) where T : BuildingSettingsDataAsset
        {
            return BuildingSettingsDataAssets.FirstOrDefault(i => i.Key == key) as T;
        }
    }
}