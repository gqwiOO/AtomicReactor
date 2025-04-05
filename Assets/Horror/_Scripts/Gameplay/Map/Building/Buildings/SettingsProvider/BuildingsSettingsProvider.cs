using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Map.Building.SettingsProvider
{
    public class BuildingsSettingsProvider : MonoBehaviour, IBuildingsSettingsProvider
    {
        [SerializeField]
        private BuildingSettingsDataAssetsCollection buildingSettingsDataAssetsCollection;
        
        public BuildingSettingsDataAsset GetBuildingSettings(string key)
        {
            return buildingSettingsDataAssetsCollection.GetByKey(key);
        }
    }
}