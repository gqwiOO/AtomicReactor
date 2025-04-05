using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Chest
{
    public class ChestBuilding: BuildingMapObject
    {
        public InventoryBuildingCore InventoryBuildingCore { get; private set; }
        
        public override async UniTask Init(Vector2Int cellPosition)
        {
            InventoryBuildingCore = new();
            base.Init(cellPosition);
        }

        public override void Tick()
        {
            
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            // TriggerUpdate();
        }

        public virtual void AddResource(int id, int amount = 1)
        {
            if (InventoryBuildingCore.Inventory.CanAdd(id, amount))
            {
                InventoryBuildingCore.Inventory.Add(id, amount);
            }
        }

        public bool CanAdd(int id, int amount = 1)
        {
            if (!IsWorking)
                return false;
            return InventoryBuildingCore.Inventory.CanAdd(id, amount);
        }
    }
}