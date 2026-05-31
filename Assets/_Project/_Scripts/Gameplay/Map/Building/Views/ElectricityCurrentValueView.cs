using System;
using Gameplay.Map.Building.Electricity;
using TMPro;
using UnityEngine;

namespace Gameplay.Buildings.View
{
    public class ElectricityCurrentValueView: MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _textField;
        
        private IElectricityContainer _electricityContainer;
        
        public void Init(IElectricityContainer electricityContainer)
        {
            if (_electricityContainer != null)
                _electricityContainer.OnValueChange -= ElectricityContainer_OnValueChange;
            _electricityContainer = electricityContainer;
            _electricityContainer.OnValueChange += ElectricityContainer_OnValueChange;

            ElectricityContainer_OnValueChange(_electricityContainer.CurrentValue);
        }

        private void ElectricityContainer_OnValueChange(float newValue)
        {
            _textField.text = newValue.ToString("F");
        }
    }
}