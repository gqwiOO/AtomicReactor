using System;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Electricity
{
    public abstract class BaseTwoItemsMechanismBuildingCore: IBuildingCore
    {
        public IItemContainer ItemInputContainer { get; private set; }
        public IItemContainer ItemOutputContainer { get; private set; }

        public BuildingSidesData BuildingSidesData { get; protected set; } =
            new BuildingSidesData(SideType.Input, SideType.None, SideType.Output, SideType.None);

        public event Action OnItemProduced;

        public BaseTwoItemsMechanismBuildingCore()
        {
            ItemInputContainer = new ItemContainer(100, 1);
            ItemOutputContainer = new ItemContainer();
        }

        public abstract void Tick(float time);
        public abstract void OnNeighbourUpdated(ICell cell, Vector2Int direction);

        public virtual void ProduceItem(int produceItemId,int inputCountExtract = 1, int outputCountPut = 1)
        {
            ItemOutputContainer.Add(produceItemId,outputCountPut);
            ItemInputContainer.Extract(inputCountExtract);
            
            OnItemProduced?.Invoke();
        }
    }
}