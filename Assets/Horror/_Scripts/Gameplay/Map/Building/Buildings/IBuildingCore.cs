using Gameplay.Inventories;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building
{
    public interface IBuildingCore
    {
        BuildingSidesData BuildingSidesData { get; }
        void Tick(float time);
        void OnNeighbourUpdated(ICell cell, Vector2Int direction);
    }

    public class InventoryBuildingCore : IBuildingCore
    {
        private readonly Inventory _inventory;

        public BuildingSidesData BuildingSidesData { get; protected set;}

        public IInventory Inventory => _inventory;

        public InventoryBuildingCore()
        {
            _inventory = new Inventory(50, 50);
        }
        
        public void Tick(float time)
        {
            
        }

        public void OnNeighbourUpdated(ICell cell, Vector2Int direction)
        {
            
        }
    }
}