using System;

namespace Gameplay.Map.Building.Electricity
{
    public interface IElectricityContainer
    {
        float CurrentValue { get; }
        float RemainingValue => MaxValue - CurrentValue;
        float MaxValue { get; }

        event Action<float> OnValueChange;
        event Action<float> OnAdded;

        void Consume(float value);
        bool CanConsume(float value);
        void Add(float value);
        void Set(float value);
    }
}