using System;
using Gameplay.Map.Building.Validator;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/BuildingSettingsDataAsset", fileName = "BuildingSettingsDataAsset")]
    public abstract class BuildingSettingsDataAsset: SerializedScriptableObject, IBuildingSettingsData
    {
        [field: SerializeField] public BuildingMapObject BuildingMapObject { get; private set; }
        [field: SerializeField] public string Key => BuildingMapObject.Key;
        [field: SerializeField] public string Name { get; set; }
        
        [field: SerializeField] public BuildingPlacementSettings BuildingPlacementSettings { get; private set; }

        [field: SerializeField] public float Price { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public bool MultiplyPlacing => BuildingPlacementSettings.MultiplyPlacing;
    }
    
    public interface IBuildingSettingsData
    {
        public BuildingMapObject BuildingMapObject { get;}
        public string Key { get; }
        public string Name { get; }
        public BuildingPlacementSettings BuildingPlacementSettings { get;  }
        public float Price { get;}
        public Sprite Icon { get;}
    }
}