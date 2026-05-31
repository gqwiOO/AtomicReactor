using System;

namespace Gameplay.Map.Building.Electricity
{
    public class ElectricityContainer : IElectricityContainer
    {
        public float CurrentValue { get; private set; }
        public float MaxValue { get; } = float.MaxValue;
        
        public float RemainingValue => MaxValue - CurrentValue;
        
        public event Action<float> OnValueChange;
        public event Action<float> OnAdded;

        public ElectricityContainer(){}

        public ElectricityContainer(float maxValue, float currentValue = 0f)
        {
            MaxValue = maxValue;
            CurrentValue = currentValue;
        }
        
        public void Consume(float value)
        {
            CurrentValue -= value;
            OnValueChange?.Invoke(CurrentValue);
        }

        public bool CanConsume(float value) => CurrentValue >= value;

        public void Add(float value)
        {
            CurrentValue += value;
            CurrentValue = Math.Min(CurrentValue, MaxValue);
            OnValueChange?.Invoke(CurrentValue);
            OnAdded?.Invoke(CurrentValue);
        }

        public void Set(float value) => CurrentValue = value;
    }
}