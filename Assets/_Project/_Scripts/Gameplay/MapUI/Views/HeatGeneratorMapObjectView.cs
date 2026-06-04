using Gameplay.Map.Building;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Generators.HeatGenerator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.MapUI.Views
{
    public class HeatGeneratorMapObjectView : BaseMapObjectView
    {
        [SerializeField] private Button _itemFuelTabButton;
        [SerializeField] private Button _fluidFuelTabButton;
        [SerializeField] private GameObject _itemFuelPanel;
        [SerializeField] private GameObject _fluidFuelPanel;
        [SerializeField] private ItemContainerCountView _fuelCellView;
        [SerializeField] private Slider _electricitySlider;
        [SerializeField] private TMP_Text _electricityValueText;

        private IElectricityContainer _electricityContainer;

        public override void Init(BuildingMapObject buildingMapObject)
        {
            var generator = (HeatGeneratorMapObject)buildingMapObject;

            if (_electricityContainer != null)
                _electricityContainer.OnValueChange -= OnElectricityChanged;

            _electricityContainer = generator.ElectricityProvider.ElectricityContainer;
            _electricityContainer.OnValueChange += OnElectricityChanged;

            _fuelCellView.Init(generator.ItemFuelContainer);

            _electricitySlider.minValue = 0f;
            _electricitySlider.maxValue = _electricityContainer.MaxValue;
            _electricitySlider.interactable = false;
            UpdateElectricityBar(_electricityContainer.CurrentValue);

            SetFuelMode(FuelMode.Item);
        }

        protected override void Start()
        {
            base.Start();
            _itemFuelTabButton.onClick.AddListener(() => SetFuelMode(FuelMode.Item));
            _fluidFuelTabButton.onClick.AddListener(() => SetFuelMode(FuelMode.Fluid));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_electricityContainer != null)
                _electricityContainer.OnValueChange -= OnElectricityChanged;
        }

        private void SetFuelMode(FuelMode mode)
        {
            _itemFuelPanel.SetActive(mode == FuelMode.Item);
            _fluidFuelPanel.SetActive(mode == FuelMode.Fluid);
        }

        private void OnElectricityChanged(float value) => UpdateElectricityBar(value);

        private void UpdateElectricityBar(float value)
        {
            _electricitySlider.value = value;
            _electricityValueText.text = $"{value:F1} / {_electricityContainer.MaxValue:F1} Ah";
        }

        private enum FuelMode { Item, Fluid }
    }
}
