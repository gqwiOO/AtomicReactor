using System;

namespace Gameplay.Map.Building
{
    public class ItemContainer : IItemContainer
    {
        public int ItemId { get; private set; }
        public int Amount { get; private set; } = 0;
        public event Action<int> OnAmountChanged;

        public ItemContainer(int startValue, int startItemId = -1)
        {
            Amount = startValue;
            ItemId = startItemId;
        }
        public ItemContainer() { }

        public void Add(int itemId, int amount = 1)
        {
            ItemId = itemId;
            Amount += amount;
            OnAmountChanged?.Invoke(Amount);
        }

        public void Extract( int amount = 1)
        {
            Amount -= amount;
            OnAmountChanged?.Invoke(Amount);
        }

        public bool CanExtract(int amount = 1) => Amount >= amount;
    }
}