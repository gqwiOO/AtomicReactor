using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Building.Generators;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.CraftBuilding
{
    public abstract class BaseElectricityRequiredBuildingCore: IBuildingCore,  IElectricBuildingCore
    {
        public IElectricityContainer ElectricityContainer { get; protected set; }

        protected BaseElectricityRequiredBuildingCore(float maxElectricityCapacity)
        {
            ElectricityContainer = new ElectricityContainer(maxElectricityCapacity);
        }
        
        protected BaseElectricityRequiredBuildingCore()
        {
            ElectricityContainer = new ElectricityContainer();
        }

        public abstract void Tick(float time);

        public virtual void OnNeighbourUpdated(ICell cell, Vector2Int direction)
        {
             CheckElectricityInput(cell,direction);
        }

        private void CheckElectricityInput(ICell cell, Vector2Int direction)
        {
            if (cell.CellVisitor is IElectricResourceBuilding electricityGenerator)
            {
                SetElectricityResourceInput(electricityGenerator.ElectricityProvider, true);
            }
        }
        
        protected void SetElectricityResourceInput(IElectricityProvider electricityProvider, bool state)
        {
            if(state)
                electricityProvider?.RegisterContainer(ElectricityContainer);
            else
                electricityProvider?.UnregisterContainer(ElectricityContainer);
        }
    }
}