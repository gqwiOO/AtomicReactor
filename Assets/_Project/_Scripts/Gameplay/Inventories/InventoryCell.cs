using System;
using UnityEngine;

namespace Gameplay.Inventories
{
	[Serializable]
    public class InventoryCell
    {
	    public int ItemId;

	    [field: SerializeField] 
	    public int Amount { get; private set; }

	    public event Action OnCellUpdated;
	    
	    public InventoryCell(int amount, int itemId)
	    {
		    Amount = amount;
		    ItemId = itemId;
	    }

	    public void Add(int amount, int itemId)
	    {
		    Amount += amount;
		    ItemId = itemId;
		    OnCellUpdated?.Invoke();
	    }

	    public void Remove(int amount)
	    {
		    Amount -= amount;
		    OnCellUpdated?.Invoke();

		    
	    }
    }
}