using Gameplay.Fuel;
using Gameplay.Map.Building;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views
{
    public class FuelContainerView : MonoBehaviour
    {
        [SerializeField] private ItemContainerCountView _itemFuelView;
        [SerializeField] private FloatContainerView _fluidFuelView;
        [SerializeField] private TMP_Text _energyText;
        [SerializeField] private Slider _energySlider;
        [SerializeField] private float _maxEnergy = 1000f;

        private FuelContainer _fuelContainer;

        public void Init(FuelContainer fuelContainer)
        {
            if (_fuelContainer != null)
                _fuelContainer.OnEnergyGained -= OnEnergyGained;

            _fuelContainer = fuelContainer;

            _itemFuelView?.Init(fuelContainer.ItemFuelContainer);
            _fluidFuelView?.Init(fuelContainer.FluidFuelContainer.FloatContainer);

            if (_energySlider != null)
            {
                _energySlider.minValue = 0f;
                _energySlider.maxValue = _maxEnergy;
                _energySlider.interactable = false;
            }

            _fuelContainer.OnEnergyGained += OnEnergyGained;
            OnEnergyGained();
        }

        private void OnDestroy()
        {
            if (_fuelContainer != null)
                _fuelContainer.OnEnergyGained -= OnEnergyGained;
        }

        private void OnEnergyGained()
        {
            float energy = _fuelContainer.CurrentEnergy;

            if (_energyText != null)
                _energyText.text = energy.ToString("F");

            if (_energySlider != null)
                _energySlider.value = energy;
        }
    }
}
