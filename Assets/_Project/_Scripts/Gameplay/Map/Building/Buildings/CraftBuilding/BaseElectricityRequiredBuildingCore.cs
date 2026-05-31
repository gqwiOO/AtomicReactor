using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Building.Generators;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.CraftBuilding
{
    public abstract class BaseElectricityRequiredBuildingCore: IBuildingCore,  IElectricBuildingCore
    {
        public BuildingSidesData BuildingSidesData { get; protected set; }
        public IElectricityContainer ElectricityContainer { get; protected set; }

        protected BaseElectricityRequiredBuildingCore()
        {
            BuildingSidesData = new BuildingSidesData(SideType.Electricity, SideType.None, SideType.Output, SideType.Input);
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
                if (BuildingSidesData.GetSide(direction) == SideType.Electricity)
                    SetElectricityResourceInput(electricityGenerator.ElectricityProvider, true);
                else
                    SetElectricityResourceInput(electricityGenerator.ElectricityProvider, false);
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