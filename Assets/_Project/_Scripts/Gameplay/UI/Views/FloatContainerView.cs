using Gameplay.Transportation.WaterPipeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views
{
    public class FloatContainerView: MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private Slider slider;

        private IFloatContainer _floatContainer;

        public void Init(IFloatContainer floatContainer)
        {
            if(_floatContainer != null)
                _floatContainer.OnValueChanged -= OnValueChanged;

            _floatContainer = floatContainer;

            if (slider != null)
            {
                slider.minValue = 0f;
                slider.maxValue = floatContainer.MaxValue;
                slider.interactable = false;
            }

            _floatContainer.OnValueChanged += OnValueChanged;
            OnValueChanged(_floatContainer.CurrentValue);
        }

        private void OnValueChanged(float value)
        {
            if (textField != null)
                textField.text = value.ToString("F");

            if (slider != null)
                slider.value = value;
        }
    }
}