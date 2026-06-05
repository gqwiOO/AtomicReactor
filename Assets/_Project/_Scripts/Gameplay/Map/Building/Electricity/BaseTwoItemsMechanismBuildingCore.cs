using System;
using Gameplay.Inventories;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Electricity
{
    public abstract class BaseTwoItemsMechanismBuildingCore: IBuildingCore
    {
        public SingleCellInventory ItemInputContainer { get; private set; }
        public SingleCellInventory ItemOutputContainer { get; private set; }

        public BuildingSidesData BuildingSidesData { get; protected set; } =
            new BuildingSidesData(SideType.Input, SideType.None, SideType.Output, SideType.None);

        public event Action OnItemProduced;

        public BaseTwoItemsMechanismBuildingCore()
        {
            ItemInputContainer = new SingleCellInventory();
            ItemOutputContainer = new SingleCellInventory();
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