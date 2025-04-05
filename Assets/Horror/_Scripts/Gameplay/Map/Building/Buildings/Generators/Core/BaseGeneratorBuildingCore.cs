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
        public BuildingSidesData BuildingSidesData => _generatorBuildingSettingsData.BuildingSidesData;
        
        public float Power => _generatorBuildingSettingsData.Power;

        public event Action<float> OnEnergyProduced;
        public IElectricityContainer ElectricityContainer { get; private set; }

        public BaseGeneratorBuildingCore(GeneratorBuildingSettingsData generatorBuildingSettingsData)
        {
            ElectricityContainer = new ElectricityContainer(generatorBuildingSettingsData.Ah_BatteryCapacity);
            _generatorBuildingSettingsData = generatorBuildingSettingsData;
        }
        
        public virtual void Tick(float time)
        {
            ElectricityContainer.Add(time * _generatorBuildingSettingsData.Power);
            OnEnergyProduced?.Invoke(time * _generatorBuildingSettingsData.Power);
            Debugging.Log(this, $"Electricity : {ElectricityContainer.CurrentValue}A");
        }

        public void OnNeighbourUpdated(ICell cell, Vector2Int direction)
        {
            
        }
    }
}