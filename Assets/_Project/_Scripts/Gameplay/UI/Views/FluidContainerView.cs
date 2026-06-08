using Gameplay.Transportation.WaterPipeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views
{
    public class FluidContainerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private Slider slider;
        [SerializeField] private Image colorImage;

        private IFluidContainer _fluidContainer;

        public void Init(IFluidContainer fluidContainer)
        {
            if (_fluidContainer != null)
            {
                _fluidContainer.FloatContainer.OnValueChanged -= OnValueChanged;
                _fluidContainer.OnAdded -= OnFluidAdded;
            }

            _fluidContainer = fluidContainer;

            if (slider != null)
            {
                slider.minValue = 0f;
                slider.maxValue = fluidContainer.FloatContainer.MaxValue;
                slider.interactable = false;
            }

            _fluidContainer.FloatContainer.OnValueChanged += OnValueChanged;
            _fluidContainer.OnAdded += OnFluidAdded;

            UpdateFluidDisplay(fluidContainer.FluidType);
            OnValueChanged(fluidContainer.CurrentValue);
        }

        private void OnFluidAdded(FluidContainer container)
        {
            UpdateFluidDisplay(container.FluidType);
            OnValueChanged(container.CurrentValue);
        }

        private void OnValueChanged(float value)
        {
            if (slider != null)
                slider.value = value;
        }

        private void UpdateFluidDisplay(FluidType fluidType)
        {
            if (_fluidContainer.LockedFluidType != FluidType.None)
            {
                if (textField != null)
                    textField.text = FluidsSettings.GetFluidName(_fluidContainer.LockedFluidType);

                if (colorImage != null)
                    colorImage.color = FluidsSettings.GetFluidColor(_fluidContainer.LockedFluidType);

                return;
            }
            
            if (fluidType == FluidType.None)
            {
                if (textField != null) textField.text = "Empty";
                if (colorImage != null) colorImage.color = Color.clear;
                return;
            }

            if (textField != null)
                textField.text = FluidsSettings.GetFluidName(fluidType);

            if (colorImage != null)
                colorImage.color = FluidsSettings.GetFluidColor(fluidType);
        }
    }
}
