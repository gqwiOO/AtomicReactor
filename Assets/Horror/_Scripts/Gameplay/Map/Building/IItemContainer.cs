using System;
using Gameplay.Inventories;

namespace Gameplay.Map.Building
{
    public interface IItemContainer: IItemStorage
    {
        int ItemId { get; }
        int Amount { get; }

        event Action<int> OnAmountChanged;
        void Extract(int amount = 1);
        bool CanExtract(int amount = 1);
    }
}