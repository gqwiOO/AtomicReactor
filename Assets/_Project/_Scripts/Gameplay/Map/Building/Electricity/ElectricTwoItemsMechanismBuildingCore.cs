using System;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Building.Furnace;
using Gameplay.Map.Building.Generators;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Electricity
{
    [Serializable]
    public class ElectricTwoItemsMechanismBuildingCoreCore : BaseTwoItemsMechanismBuildingCore, IElectricBuildingCore
    {
        private TwoItemsMechanismSettingsData _twoItemsMechanismSettingsData;
        public IElectricityContainer ElectricityContainer { get; private set; }

        private float _energySpent = 0f;
        private IElectricityProvider _electricityResource;
        
        public ElectricTwoItemsMechanismBuildingCoreCore(TwoItemsMechanismSettingsData twoItemsMechanismSettingsData)
        {
            _twoItemsMechanismSettingsData = twoItemsMechanismSettingsData;
            ElectricityContainer = new ElectricityContainer();
        }
        
        public override void Tick(float time)
        {
            var buildingEnergyPowerPerTick = _twoItemsMechanismSettingsData.BuildingEnergyPower * time * ElectricityTool.Voltage;
            if (!ElectricityContainer.CanConsume(buildingEnergyPowerPerTick))
                return;
            
            if (!ElectricityContainer.CanConsume(buildingEnergyPowerPerTick) || !ItemInputContainer.CanExtract(1))
                return;
            
            ElectricityContainer.Consume(buildingEnergyPowerPerTick);
            _energySpent += buildingEnergyPowerPerTick * _twoItemsMechanismSettingsData.Speed;
            
            if (_energySpent >= _twoItemsMechanismSettingsData.EnergySpentForOneItem)
            {
                ProduceItem(GetOutputItemKey(ItemInputContainer.ItemId),1,1);
                _energySpent = 0f;
            }
        }

        public void SetElectricityResourceInput(IElectricityProvider electricityProvider, bool state)
        {
            if(state)
                electricityProvider?.RegisterContainer(ElectricityContainer);
            else
                electricityProvider?.UnregisterContainer(ElectricityContainer);
        }

        public void CheckElectricityInput(ICell cell, Vector2Int direction)
        {
            if (cell.CellVisitor is IElectricResourceBuilding electricityGenerator)
            {
                if (BuildingSidesData.GetSide(direction) == SideType.Electricity)
                    SetElectricityResourceInput(electricityGenerator.ElectricityProvider, true);
                
                else
                    SetElectricityResourceInput(electricityGenerator.ElectricityProvider, false);
            }
        }

        private int GetOutputItemKey(int inputItemKey) => _twoItemsMechanismSettingsData.GetOutputItemOfMechanism(inputItemKey);

        public override void OnNeighbourUpdated(ICell cell, Vector2Int direction) => CheckElectricityInput(cell, direction);
    }

    public interface IElectricBuildingCore
    {
        public IElectricityContainer ElectricityContainer { get; }
    }
}