using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Core.Services.UpdateService;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using NUnit.Framework;
using Sirenix.Utilities;

namespace Gameplay.Map.Wires
{
    [Serializable]
    public class WireSystem: IUpdatable
    {
        public HashSet<Wire> Wires;

        private HashSet<IElectricityProvider> ElectricityProviders;
        private HashSet<IElectricityContainer> ElectricityContainers;

        private IElectricityContainer ElectricityContainer;
        private IUpdateService _updateService;

        public UpdateType UpdateType => UpdateType.Update;
        
        public WireSystem(IUpdateService updateService)
        {
            _updateService = updateService;
            ElectricityContainer = new ElectricityContainer();
            Wires = new HashSet<Wire>();

            ElectricityProviders = new HashSet<IElectricityProvider>();
            ElectricityContainers = new HashSet<IElectricityContainer>();
            _updateService.Add(this);
        }
        
        public WireSystem(IEnumerable<Wire> wires)
        {
            ElectricityContainer = new ElectricityContainer();
            Wires = new HashSet<Wire>(wires);
            
            ElectricityProviders = new HashSet<IElectricityProvider>();
            ElectricityContainers = new HashSet<IElectricityContainer>();
        }
        public void Tick()
        {
            GetEnergyFromProviders();
            SendEnergyToContainers();
        }

        private void GetEnergyFromProviders()
        {
            float totalEnergy = ElectricityProviders.Sum(item => item.ElectricityContainer.CurrentValue);
            ElectricityProviders.ForEach(item => item.ExtractAllEnergy());
            ElectricityContainer.Set(totalEnergy);
        }

        public void AddElectricityProvider(IElectricityProvider electricityProvider)
        {
            ElectricityProviders.Add(electricityProvider);
        }

        public void AddElectricityProvidersRange(IEnumerable<IElectricityProvider> electricityProvider) 
            => ElectricityProviders.AddRange(electricityProvider);

        public void RemoveElectricityProvider(IElectricityProvider electricityProvider)
        {
            ElectricityProviders.Remove(electricityProvider);
        }


        public void AddElectricityContainer(IElectricityContainer electricityProvider) 
            => ElectricityContainers.Add(electricityProvider);

        public void AddElectricityContainersRange(IEnumerable<IElectricityContainer> electricityProvider) 
            => ElectricityContainers.AddRange(electricityProvider);


        public void RemoveElectricityContainer(IElectricityContainer electricityProvider) 
            => ElectricityContainers.Remove(electricityProvider);
        
        private void SendEnergyToContainers()
        {
            float totalEnergy = ElectricityContainer.CurrentValue;
            if (totalEnergy <= 0) return; 

            float energyPerContainer = totalEnergy / ElectricityContainers.Count;

            foreach (var container in ElectricityContainers)
                container.Add(energyPerContainer);

            ElectricityContainer.Consume(totalEnergy);
        }
    }

    [Serializable]
    public class Wire
    {
        
    }
}