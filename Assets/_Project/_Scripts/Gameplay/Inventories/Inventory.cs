using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Debugging;
using Gameplay.Map.Building;

namespace Gameplay.Inventories
{
    [Serializable]
    public class Inventory: IInventory
    {
        public int Capacity;
        public int CellCapacity;
        protected List<InventoryCell> Cells;

        public IEnumerable<InventoryCell> InventoryCells => Cells;

        public int GetItemCount(int itemId)
        {
            int result = 0;
            foreach (InventoryCell inventoryCell in Cells)
            {
                if (inventoryCell.ItemId == itemId)
                {
                    result += inventoryCell.Amount;
                }
            }

            return result;
        }

        public event Action OnChanged;

        public Inventory(int capacity, int cellCapacity)
        {
            Capacity = capacity;
            CellCapacity = cellCapacity;
            Cells = new List<InventoryCell>(Capacity);
            for (int i = 0; i < capacity; i++)
            {
                Cells.Add(new InventoryCell(0,-1));
            }
        }

        public void Add(int itemId, int amount = 1)
        {
            foreach (var cell in Cells.Where(c => (c.ItemId == itemId && c.Amount < CellCapacity) || 
                                                  (c.ItemId == -1&& c.Amount < CellCapacity)))
            {
                int spaceLeft = CellCapacity - cell.Amount;
                int toAdd = Math.Min(spaceLeft, amount);
                cell.Add(toAdd,itemId);
                amount -= toAdd;
                if (amount <= 0) return;
            }

            while (amount > 0 && Cells.Count < Capacity)
            {
                int toAdd = Math.Min(amount, CellCapacity);
                Cells.Add(new InventoryCell(toAdd,itemId));
                amount -= toAdd;
            }
            
            OnChanged?.Invoke();
        }

        public int Extract(int itemId, int amount = 1)
        {
            for (int i = Cells.Count - 1; i >= 0 && amount > 0; i--)
            {
                if (Cells[i].ItemId == itemId)
                {
                    int toRemove = Math.Min(amount, Cells[i].Amount);
                    Cells[i].Remove(toRemove);
                    amount -= toRemove;
                    OnChanged?.Invoke();
                    return toRemove;
                }
            }

            OnChanged?.Invoke();
            return 0;
        }

        public bool HasEnough(int itemId, int amount = 1)
        {
            return Cells.Where(c => c.ItemId == itemId).Sum(c => c.Amount) >= amount;
        }

        public bool CanAdd(int itemId, int amount)
        {
            int remainingAmount = amount;

            foreach (var cell in Cells.Where(c => (c.ItemId == itemId && c.Amount < CellCapacity) || 
                                                  (c.ItemId == -1 && c.Amount < CellCapacity)))
            {
                int spaceLeft = CellCapacity - cell.Amount;
                remainingAmount -= Math.Min(spaceLeft, remainingAmount);
                if (remainingAmount <= 0) return true;
            }

            int availableSlots = Capacity - Cells.Count;
            int requiredSlots = (int)Math.Ceiling((float)remainingAmount / CellCapacity);

            return requiredSlots <= availableSlots;
        }
    }

    public interface IInventory: IItemStorage
    {
        IEnumerable<InventoryCell> InventoryCells { get; }
        bool HasEnough(int itemId, int amount = 1);
        int Extract(int itemId, int amount = 1);
        bool CanAdd(int itemId, int amount);

        int GetItemCount(int itemId);
        
        event Action OnChanged;
    }

    public interface IItemStorage
    {
        void Add(int itemId, int amount = 1);
    }
}