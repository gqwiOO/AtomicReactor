using System;
using Gameplay.Map.Building.Furnace;
using UnityEngine;

namespace Gameplay.Map.Building.Generators
{
    [Serializable]
    public class GeneratorBuildingSettingsData: BuildingSettingsData
    {
        [Range(0f, 1f)]
        [Tooltip("Fraction of fuel energy converted to electricity (0=0%, 1=100%)")]
        public float Efficiency = 0.8f;
        
        public float Power;
        public float Ah_BatteryCapacity;

        public BuildingSidesData BuildingSidesData;
    }
}