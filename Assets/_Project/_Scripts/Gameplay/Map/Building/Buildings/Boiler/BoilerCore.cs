using System;
using System.Collections.Generic;
using Gameplay.Fuel;
using Gameplay.Map.Cell;
using Gameplay.Transportation.WaterPipeSystem;
using UnityEngine;

namespace Gameplay.Map.Building.Boiler
{
    public class BoilerCore : IBuildingCore
    {
        public FuelContainer FuelContainer { get; }
        public FluidProvider SteamContainer { get; }
        public IFloatContainer WaterContainer { get; }

        public BuildingSidesData BuildingSidesData { get; }

        private readonly float _requiredHeatPerWaterUnit;
        private readonly float _requiredWaterPerSteamUnit;

        public BoilerCore(
            List<FuelDataAsset> acceptedFuels,
            int fuelCapacity,
            int waterCapacity,
            int steamCapacity,
            float requiredHeatPerWaterUnit,
            float requiredWaterPerSteamUnit,
            BuildingSidesData buildingSidesData)
        {
            FuelContainer = new FuelContainer(acceptedFuels, fuelCapacity);
            WaterContainer = new FloatContainer(waterCapacity);
            SteamContainer = new FluidProvider(FluidType.Steam, steamCapacity);
            _requiredHeatPerWaterUnit = requiredHeatPerWaterUnit;
            _requiredWaterPerSteamUnit = requiredWaterPerSteamUnit;
            BuildingSidesData = buildingSidesData;
        }

        public void Tick(float time)
        {
            FuelContainer.BurnFuel(tryItems: true);
            TryConvertToSteam();
        }

        private void TryConvertToSteam()
        {
            if (FuelContainer.CurrentEnergy <= 0f) return;
            if (WaterContainer.CurrentValue <= 0f) return;
            if (SteamContainer.FloatContainer.CurrentValue >= SteamContainer.FloatContainer.MaxValue) return;

            float waterFromEnergy = FuelContainer.CurrentEnergy / _requiredHeatPerWaterUnit;
            float waterToConsume = Math.Min(waterFromEnergy, WaterContainer.CurrentValue);

            float steamToAdd = waterToConsume / _requiredWaterPerSteamUnit;
            float steamSpace = SteamContainer.FloatContainer.MaxValue - SteamContainer.CurrentValue;
            steamToAdd = Math.Min(steamToAdd, steamSpace);

            if (steamToAdd <= 0f) return;

            waterToConsume = steamToAdd * _requiredWaterPerSteamUnit;
            float energyConsumed = waterToConsume * _requiredHeatPerWaterUnit;

            FuelContainer.ConsumeEnergy(energyConsumed);
            WaterContainer.Remove(waterToConsume);
            SteamContainer.AddFluid(FluidType.Steam, steamToAdd);
        }

        public void OnNeighbourUpdated(ICell cell, Vector2Int direction)
        {
        }
    }
}
