using System.Collections.Generic;
using Gameplay.Fuel;
using Gameplay.Map.Building.Generators;
using UnityEngine;

namespace Gameplay.Map.Building.Boiler
{
    [CreateAssetMenu(menuName = "Core/Buildings/BoilerBuildingSettingsDataAsset", fileName = "BoilerBuildingSettingsDataAsset")]
    public class BoilerBuildingSettingsDataAsset: BuildingSettingsDataAsset
    {
        [SerializeField] private float requiredHeatPerWaterUnit;
        public float RequiredHeatPerWaterUnit => requiredHeatPerWaterUnit;
        [field: SerializeField] public float M3_RequiredWaterPerSteamM3 { get; private set; }

        [field: SerializeField] public BuildingSidesData BuildingSidesData { get; private set; }
        [field: SerializeField] public List<FuelDataAsset> AcceptedFuels { get; private set; }
        [field: SerializeField] public int FuelCapacity { get; private set; }
        [field: SerializeField] public int M3_WaterCapacity { get; private set; }
        [field: SerializeField] public int M3_SteamCapacity { get; private set; }
    }
}