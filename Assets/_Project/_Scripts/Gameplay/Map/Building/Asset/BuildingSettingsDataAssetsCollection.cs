using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/BuildingSettingsDataAssetsCollection", fileName = "BuildingSettingsDataAssetsCollection")]

    public class BuildingSettingsDataAssetsCollection : ScriptableObject
    {
        [field: SerializeField] public List<BuildingSettingsDataAsset> Collection { get; private set; }

        public BuildingSettingsDataAsset GetByKey(string key)
        {
            return Collection.FirstOrDefault(i => i.Key == key);
        }
        public T GetByKey<T>(string key) where T : BuildingSettingsDataAsset
        {
            return Collection.FirstOrDefault(i => i.Key == key) as T;
        }
        
        [Button]
        private void GetAllAssets()
        {
            var guids = AssetDatabase.FindAssets("t:BuildingSettingsDataAsset");
            var assets = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<BuildingSettingsDataAsset>)
                .Where(asset => asset != null)
                .ToList();

            Collection = assets;
        }
    }
}