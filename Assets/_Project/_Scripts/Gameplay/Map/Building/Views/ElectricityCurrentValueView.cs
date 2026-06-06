using Gameplay.Map.Building.Electricity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Buildings.View
{
    public class ElectricityCurrentValueView: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _textField;

        [SerializeField]
        private Slider _slider;

        private IElectricityContainer _electricityContainer;

        public void Init(IElectricityContainer electricityContainer)
        {
            if (_electricityContainer != null)
                _electricityContainer.OnValueChange -= ElectricityContainer_OnValueChange;
            _electricityContainer = electricityContainer;
            _electricityContainer.OnValueChange += ElectricityContainer_OnValueChange;

            if (_slider != null)
                _slider.maxValue = _electricityContainer.MaxValue;

            ElectricityContainer_OnValueChange(_electricityContainer.CurrentValue);
        }

        private void ElectricityContainer_OnValueChange(float newValue)
        {
            if (_textField != null)
                _textField.text = newValue.ToString("F");

            if (_slider != null)
                _slider.value = newValue;
        }
    }
}