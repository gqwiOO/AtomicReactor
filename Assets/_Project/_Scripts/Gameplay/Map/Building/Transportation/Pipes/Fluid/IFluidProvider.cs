using System;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public interface IFluidProvider
    {
        FluidType FluidType { get; }
        float CurrentValue { get; }
        IFloatContainer FloatContainer { get; }
        void ExtractFluid(float amount);
        void AddFluid(FluidType fluidType,float amount);

        event Action<FluidProvider> OnAdded;
    }

    public class FluidProvider : IFluidProvider
    {
        public FluidType FluidType { get; private set; } = FluidType.None;
        public float CurrentValue => _floatContainer.CurrentValue;

        private FloatContainer _floatContainer;
        public IFloatContainer FloatContainer => _floatContainer;

        public FluidProvider()
        {
            _floatContainer = new FloatContainer();
        }
        public void ExtractFluid(float amount)
        {
            _floatContainer.Remove(amount);
        }

        public void AddFluid(FluidType fluidType, float amount)
        {
            if (FluidType != fluidType && FluidType != FluidType.None)
                return;
            
            FluidType = fluidType;
            _floatContainer.Add(amount);
            OnAdded?.Invoke(this);
        }

        public event Action<FluidProvider> OnAdded;
    }
}