using System;
using System.Collections.Generic;
using Gameplay.Fuel;
using Gameplay.Map.Building.Generators;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Map.Building.Generators.HeatGenerator
{
    [Serializable]
    public class HeatGeneratorSettingsData : GeneratorBuildingSettingsData
    {
        [Range(0f, 1f)]
        [Tooltip("Fraction of fuel energy converted to electricity (0=0%, 1=100%)")]
        public float Efficiency = 0.8f;

        [Title("Fuel")]
        [Tooltip("Which fuels this generator can burn")]
        public List<FuelDataAsset> AcceptedFuels;

        [Tooltip("Number of item slots in the internal solid-fuel hopper")]
        public int ItemFuelSlots = 1;

        [Tooltip("Internal fluid/gas fuel tank capacity in litres")]
        public float FluidFuelTankCapacity = 100f;
    }
    
    [Serializable]
    public class SteamGeneratorSettingsData : GeneratorBuildingSettingsData
    {
        public int SteamContainerCapacity;
        public int Capacity;
    }
    
    [Serializable]
    public class WindGeneratorSettingsData : GeneratorBuildingSettingsData
    {
    }
}
