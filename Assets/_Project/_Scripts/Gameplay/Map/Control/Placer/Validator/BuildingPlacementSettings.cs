using System.Collections.Generic;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Validator
{
    [System.Serializable]
    public class BuildingPlacementSettings
    {
        [field: SerializeField]
        public List<BuildingPlacementSettingData> CellTypeValidation { get; private set; }

        [field: SerializeField] public bool MultiplyPlacing { get; set; }
        
    }
    
    [System.Serializable]
    public class BuildingPlacementSettingData
    {
        [field: SerializeField]
        public BuildingPlacementValidationType BuildingPlacementValidationType { get; private set; }
        
        [field: SerializeField]
        public List<CellType> CellTypes { get; private set; }
    }
    
    public enum BuildingPlacementValidationType
    {
        None = 0, 
        AnyNeighbour = 1,
        BuildingCell = 2,
        
    }
}