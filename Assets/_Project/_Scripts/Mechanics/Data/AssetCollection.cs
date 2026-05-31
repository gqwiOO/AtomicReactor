using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Mechanics.Data
{
    public class AssetCollection<TObject>: ScriptableObject where TObject: ScriptableObject
    {
        [SerializeField] private List<TObject> assets = new List<TObject>();
        public IEnumerable<TObject> Assets => assets;
        
        [Button]
        private void GetAllAssets()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(TObject).Name}");
            assets = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<TObject>)
                .Where(asset => asset != null)
                .ToList();

            EditorUtility.SetDirty(this);
        }
    }
}