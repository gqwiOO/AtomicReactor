using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Debugging;
using Gameplay.Crafting;
using Gameplay.Inventories;
using Gameplay.Map.Building.Chest;
using Gameplay.Map.Building.Generators;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.CraftBuilding
{
    [Serializable]
    public class CraftBuildingCore :  BaseElectricityRequiredBuildingCore
    {
        private IInventory _inventoryOutside;
        private IInventory _inventoryInside;
        private CraftData _craftData;
        private float _craftTimer;
        
        public SingleCellInventory ItemOutputContainer { get; private set; }
        public IInventory InputInventory => _inventoryInside;

        public CraftBuildingCore() : base()
        {
            _inventoryInside = new Inventory(10,10);
            ItemOutputContainer = new SingleCellInventory();
        }
        
        public void SetCraftData(CraftData craftData)
        {
            _craftData = craftData;
        }
        
        public override void Tick(float time)
        {
            if (_craftData == null || _inventoryInside == null) return;
            //5min
            ExtractItemsFromOutsideInventory();
            if (!CraftTool.CanCraft(_inventoryInside, _craftData)) return;
            
            _craftTimer += time;
            if (_craftTimer >= GetCraftTime())
            {
                Craft();
                _craftTimer = 0;
            }
        }

        private void ExtractItemsFromOutsideInventory()
        {
            if (_inventoryOutside == null) 
                return;
            
            //todo: replace list
            List<(int, int)> extractedItems = CraftTool.ExtractAllRequiredCraftItemsFromInventory(_inventoryOutside, _craftData).ToList();
            
            foreach (var (itemId, amount) in extractedItems)
            {
                _inventoryInside.Add(itemId, amount);
            }
        }

        private void Craft()
        {
            Debugging.Log(this,$"Item crafted : {_craftData.Outputs.First().ItemId} with amount {_craftData.Outputs.First().Amount}");

            foreach (var output in _craftData.Inputs)
                _inventoryInside.Extract(output.ItemId, output.Amount);
            
            foreach (var output in _craftData.Outputs)
                ItemOutputContainer.Add(output.ItemId, output.Amount);
        }

        private float GetCraftTime() => 0.1f;

        public override void OnNeighbourUpdated(ICell cell, Vector2Int direction)
        {
            base.OnNeighbourUpdated(cell, direction);
            CheckInputContainer(cell, direction);
        }

        private void CheckInputContainer(ICell cell, Vector2Int direction)
        {
            if (cell.CellVisitor is ChestBuilding chestBuilding)
            {
                if (BuildingSidesData.GetSide(direction) == SideType.Input)
                {
                    SetInputContainer(chestBuilding.InventoryBuildingCore.Inventory);
                }
            }
        }

        private void SetInputContainer(IInventory inventory)
        {
            _inventoryOutside = inventory;
        }
    }
}