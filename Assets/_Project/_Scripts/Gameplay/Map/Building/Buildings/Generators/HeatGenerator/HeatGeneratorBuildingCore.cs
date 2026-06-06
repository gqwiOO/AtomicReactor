using System;
using Gameplay.Fuel;

namespace Gameplay.Map.Building.Generators.HeatGenerator
{
    public class HeatGeneratorBuildingCore : BaseGeneratorBuildingCore
    {
        private readonly HeatGeneratorSettingsData _settings;

        public FuelContainer FuelContainer { get; }
        public bool IsRunning => _burnEnergyJoules > 0f;

        private float _burnEnergyJoules;

        public HeatGeneratorBuildingCore(HeatGeneratorSettingsData settings)
            : base(settings)
        {
            _settings = settings;
            FuelContainer = new FuelContainer(settings.AcceptedFuels, settings.ItemFuelSlots);
        }

        public override void Tick(float time)
        {
            FuelContainer.BurnFuel(tryItems: _burnEnergyJoules <= 0f);

            if (_burnEnergyJoules <= 0f)
                return;

            float energyNeeded = time * KwPower / _settings.Efficiency;
            float energyConsumed = Math.Min(_burnEnergyJoules, energyNeeded);
            _burnEnergyJoules -= energyConsumed;

            float electricity = energyConsumed * _settings.Efficiency;
            ElectricityContainer.Add(electricity);
            RaiseOnEnergyProduced(electricity);
        }
    }
}
