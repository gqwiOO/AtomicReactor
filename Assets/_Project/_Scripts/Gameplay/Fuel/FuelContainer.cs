using System;
using System.Collections.Generic;
using Gameplay.Inventories;
using Gameplay.Transportation.WaterPipeSystem;

namespace Gameplay.Fuel
{
    public class FuelContainer
    {
        public SingleCellInventory ItemFuelContainer { get; }
        public IFluidProvider FluidFuelProvider { get; }
        
        public float CurrentEnergy { get; private set; }
        
        public event Action OnEnergyGained;
        
        private readonly List<FuelDataAsset> _acceptedFuels;

        public FuelContainer(List<FuelDataAsset> acceptedFuels, int itemFuelSlots)
        {
            _acceptedFuels = acceptedFuels;
            ItemFuelContainer = new SingleCellInventory(itemFuelSlots);
            FluidFuelProvider = new FluidProvider();
        }
        
        public bool IsEmpty() => ItemFuelContainer.Amount == 0 && FluidFuelProvider.CurrentValue <= 0f;

        public float ConsumeEnergy(float amount)
        {
            float consumed = Math.Min(CurrentEnergy, amount);
            CurrentEnergy -= consumed;
            return consumed;
        }

        public bool IsAcceptedItemFuel(int itemId)
        {
            if (_acceptedFuels == null) return false;
            foreach (var fuel in _acceptedFuels)
            {
                if (fuel.SourceType == FuelSourceType.Item && fuel.ItemAsset?.ItemId == itemId)
                    return true;
            }
            return false;
        }

        public bool CanAddItemFuel(int itemId, int amount)
        {
            if (!IsAcceptedItemFuel(itemId)) return false;
            return ItemFuelContainer.CanAdd(itemId, amount);
        }

        public void BurnFuel(bool tryItems)
        {
            BurnFluid();
            if (tryItems) BurnItem();
        }

        private void BurnFluid()
        {
            if (_acceptedFuels == null) return;

            float available = FluidFuelProvider.CurrentValue;
            if (available <= 0f) return;

            foreach (var fuel in _acceptedFuels)
            {
                if (fuel.SourceType == FuelSourceType.Item) continue;
                if (fuel.FluidType != FluidFuelProvider.FluidType) continue;

                FluidFuelProvider.ExtractFluid(available);
                CurrentEnergy += available * fuel.EnergyInJoules;
                OnEnergyGained?.Invoke();
                break;
            }
        }

        private void BurnItem()
        {
            if (_acceptedFuels == null) return;
            if (!ItemFuelContainer.HasEnough()) return;

            foreach (var fuel in _acceptedFuels)
            {
                if (fuel.SourceType != FuelSourceType.Item) continue;
                if (fuel.ItemAsset == null) continue;
                if (ItemFuelContainer.ItemId != fuel.ItemAsset.ItemId) continue;

                CurrentEnergy += 1 * fuel.EnergyInJoules;
                ItemFuelContainer.Extract(ItemFuelContainer.ItemId);
                OnEnergyGained?.Invoke();
                break;
            }
        }
    }
}
