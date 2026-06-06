using System;
using Core.Scripts.Debugging;
using Core.Scripts.Debugging.Logging;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Generators
{
    public class BaseGeneratorBuildingCore : IBuildingCore
    {
        private readonly GeneratorBuildingSettingsData _generatorBuildingSettingsData;
        
        public float KwPower => _generatorBuildingSettingsData.KW_Power;

        public event Action<float> OnEnergyProduced;
        public IElectricityContainer ElectricityContainer { get; private set; }

        public BaseGeneratorBuildingCore(GeneratorBuildingSettingsData generatorBuildingSettingsData)
        {
            ElectricityContainer = new ElectricityContainer(generatorBuildingSettingsData.KWH_BatteryCapacity);
            _generatorBuildingSettingsData = generatorBuildingSettingsData;
        }
        
        public virtual void Tick(float time)
        {
            ElectricityContainer.Add(time * _generatorBuildingSettingsData.KW_Power);
            RaiseOnEnergyProduced(time * _generatorBuildingSettingsData.KW_Power);
            Debugging.Log(this, $"Electricity : {ElectricityContainer.CurrentValue}A");
        }

        public float ApplyEfficiency(float power) => power * _generatorBuildingSettingsData.Efficiency;

        protected void RaiseOnEnergyProduced(float value) => OnEnergyProduced?.Invoke(value);

        public void OnNeighbourUpdated(ICell cell, Vector2Int direction)
        {
            
        }
    }
}