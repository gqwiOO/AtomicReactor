using System;
using UnityEngine;

namespace Gameplay.Map.Building
{
    public class BuildingMapSpawnSelector : MonoBehaviour, IBuildingMapSpawnSelector
    {
        [SerializeField] private BuildingSettingsDataAssetsCollection _buildingsAssetCollection;
        
        public event Action<BuildingSettingsDataAsset> OnBuildingChanged;

        public BuildingSettingsDataAsset CurrentBuilding { get; private set; }

        public void Select(string key)
        {
            CurrentBuilding = _buildingsAssetCollection.GetByKey(key);
            OnBuildingChanged?.Invoke(CurrentBuilding);
        }
        
        public void Unselect()
        {
            CurrentBuilding = null;
        }
    }
}