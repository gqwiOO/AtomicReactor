using System;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public interface IFloatContainer
    {
        float CurrentValue { get; }
        public float MaxValue { get; }
        void Add(float value);
        void Remove(float value);

        event Action<float> OnValueChanged;
    }

    public class FloatContainer : IFloatContainer
    {
        public float CurrentValue { get; private set; }
        public float MaxValue { get; private set; }

        public FloatContainer(float maxValue = Int32.MaxValue)
        {
            MaxValue = maxValue;
        }
        
        public void Add(float value)
        {
            CurrentValue += value;
            CurrentValue = Math.Min(CurrentValue, MaxValue);
            OnValueChanged?.Invoke(CurrentValue);
        }

        public void Remove(float value)
        {
            CurrentValue -= value;
            CurrentValue = Math.Max(CurrentValue, 0);
            OnValueChanged?.Invoke(CurrentValue);
        }

        public event Action<float> OnValueChanged;
    }
}