namespace Gameplay.Inventories
{
    public class SingleCellInventory : Inventory
    {
        public SingleCellInventory(int cellCapacity = 256): base(1, cellCapacity) { }
        
        public int ItemId => Cells[0].ItemId;
        
        public bool HasEnough(int amount = 1) 
            => Cells[0].Amount >= amount;
        
        public int Amount => Cells[0].Amount;
    }
}