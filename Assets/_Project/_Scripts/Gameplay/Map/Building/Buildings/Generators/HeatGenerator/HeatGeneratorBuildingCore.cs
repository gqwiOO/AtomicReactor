using System;
using Gameplay.Fuel;
using Gameplay.Inventories;
using Gameplay.Transportation.WaterPipeSystem;

namespace Gameplay.Map.Building.Generators.HeatGenerator
{
    public class HeatGeneratorBuildingCore : BaseGeneratorBuildingCore
    {
        private readonly HeatGeneratorSettingsData _settings;

        public SingleCellInventory ItemFuelContainer { get; }
        public IFluidProvider FluidFuelProvider { get; }

        public bool IsRunning => _burnEnergyJoules > 0f;
        public int ItemFuelCapacity => _settings.ItemFuelSlots;

        private float _burnEnergyJoules;

        public bool IsAcceptedItemFuel(int itemId)
        {
            if (_settings.AcceptedFuels == null) return false;
            foreach (var fuel in _settings.AcceptedFuels)
            {
                if (fuel.SourceType == FuelSourceType.Item && fuel.ItemAsset?.ItemId == itemId)
                    return true;
            }
            return false;
        }

        public bool CanAddItemFuel(int itemId, int amount)
        {
            if (!IsAcceptedItemFuel(itemId)) return false;
            if (ItemFuelContainer.Amount > 0 && ItemFuelContainer.ItemId != itemId) return false;
            return ItemFuelContainer.Amount + amount <= ItemFuelCapacity;
        }

        public HeatGeneratorBuildingCore(HeatGeneratorSettingsData settings)
            : base(settings)
        {
            _settings = settings;
            ItemFuelContainer = new SingleCellInventory();
            FluidFuelProvider = new FluidProvider();
        }

        public override void Tick(float time)
        {
            RefillFromFluid();
            RefillFromItems();

            if (_burnEnergyJoules <= 0f)
                return;

            float energyNeeded = time * Power / _settings.Efficiency;
            float energyConsumed = Math.Min(_burnEnergyJoules, energyNeeded);
            _burnEnergyJoules -= energyConsumed;

            float electricity = energyConsumed * _settings.Efficiency;
            ElectricityContainer.Add(electricity);
            RaiseOnEnergyProduced(electricity);
        }

        private void RefillFromFluid()
        {
            if (_settings.AcceptedFuels == null) return;

            float available = FluidFuelProvider.CurrentValue;
            if (available <= 0f) return;

            foreach (var fuel in _settings.AcceptedFuels)
            {
                if (fuel.SourceType == FuelSourceType.Item) continue;
                if (fuel.FluidType != FluidFuelProvider.FluidType) continue;

                _burnEnergyJoules += available * fuel.EnergyInJoules;
                FluidFuelProvider.ExtractFluid(available);
                break;
            }
        }

        private void RefillFromItems()
        {
            if (_burnEnergyJoules > 0f) return;
            if (_settings.AcceptedFuels == null) return;
            if (!ItemFuelContainer.HasEnough()) return;

            foreach (var fuel in _settings.AcceptedFuels)
            {
                if (fuel.SourceType != FuelSourceType.Item) continue;
                if (fuel.ItemAsset == null) continue;
                if (ItemFuelContainer.ItemId != fuel.ItemAsset.ItemId) continue;

                ItemFuelContainer.Extract(ItemFuelContainer.ItemId);
                _burnEnergyJoules += fuel.EnergyInJoules;
                break;
            }
        }
    }
}
