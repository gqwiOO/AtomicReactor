using System;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public interface IFluidContainer
    {
        FluidType FluidType { get; }
        float CurrentValue { get; }
        IFloatContainer FloatContainer { get; }
        FluidType LockedFluidType { get; } 
        void ExtractFluid(float amount);
        void AddFluid(FluidType fluidType,float amount);
        bool IsEmpty();
        event Action<FluidContainer> OnAdded;
    }

    public class FluidContainer : IFluidContainer
    {
        public FluidType FluidType { get; private set; } = FluidType.None;
        public float CurrentValue => _floatContainer.CurrentValue;

        private FloatContainer _floatContainer;
        public IFloatContainer FloatContainer => _floatContainer;

        public FluidType LockedFluidType { get; private set; } 

        public FluidContainer(FluidType lockFluidType = FluidType.None, float capacity = 0)
        {
            LockedFluidType = lockFluidType;
            _floatContainer = new FloatContainer(capacity);
        }
        public void ExtractFluid(float amount)
        {
            _floatContainer.Remove(amount);
        }

        public void AddFluid(FluidType fluidType, float amount)
        {
            if (FluidType != fluidType && FluidType != FluidType.None)
                return;

            if (LockedFluidType != fluidType && LockedFluidType != FluidType.None)
                return;
            
            FluidType = fluidType;
            _floatContainer.Add(amount);
            OnAdded?.Invoke(this);
        }

        public bool IsEmpty() => CurrentValue <= 0;

        public event Action<FluidContainer> OnAdded;
    }

    public interface IFluidExtractionSource
    {
        IFluidContainer ExtractionFluidContainer { get; }
    }

    public interface IFluidMultiExtractionSource
    {
        IFluidContainer ExtractionFluidContainer_1 { get; }
        IFluidContainer ExtractionFluidContainer_2 { get; }
    }
}