using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building.Electricity.Consumer;

namespace Gameplay.Map.Building.Electricity
{
    public class ElectricityProvider : IElectricityProvider
    {
        private HashSet<IElectricityContainer> _containers = new ();
        
        public IElectricityContainer ElectricityContainer { get; private set; }

        public void SetContainer(IElectricityContainer electricityContainer)
        {
            ElectricityContainer = electricityContainer;
            
            ElectricityContainer.OnAdded += ElectricityContainer_OnValueChange;
        }

        private void ElectricityContainer_OnValueChange(float obj)
        {
            IEnumerable<Tuple<IElectricityContainer,float>> splitEnergy =  SplitEnergyForContainers();

            ExtractEnergyToContainers(splitEnergy);
        }

        private IEnumerable<Tuple<IElectricityContainer,float>> SplitEnergyForContainers()
        {
            float singleContainerEnergyValue = ElectricityContainer.CurrentValue / _containers.Count;
            foreach (var container in _containers)
                yield return new Tuple<IElectricityContainer, float>(container,Math.Min(singleContainerEnergyValue,container.RemainingValue));
        }

        private void ExtractEnergyToContainers(IEnumerable<Tuple<IElectricityContainer, float>> splitEnergyByContainers)
        {
            foreach (var containerEnergyPair in splitEnergyByContainers)
            {
                containerEnergyPair.Item1.Add(containerEnergyPair.Item2);
                ElectricityContainer.Consume(containerEnergyPair.Item2);
            }
        }
        
        public void RegisterContainer(IElectricityContainer electricityContainer) 
            => _containers.Add(electricityContainer);

        public void UnregisterContainer(IElectricityContainer electricityContainer)
            => _containers.Remove(electricityContainer);

        public void ExtractAllEnergy()
        {
            ElectricityContainer.Consume(ElectricityContainer.CurrentValue);
        }

        // public void ConsumeElectricity(float value)
        // {
            // ElectricityContainer.Consume(value);
        // }

        // public bool CanConsume(float value)
        // {
            // return ElectricityContainer.CurrentValue >= value;
        // }
    }
}