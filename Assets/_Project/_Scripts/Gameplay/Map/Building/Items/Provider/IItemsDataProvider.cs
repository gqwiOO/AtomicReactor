using System.Collections.Generic;
using Gameplay.Map.Building.Items.Data;
using UnityEngine;

namespace Gameplay.Map.Building.Items.Provider
{
    public interface IItemsDataProvider
    {
        Sprite GetItemSprite(int id);

        IEnumerable<IItemData> GetAllItems();
    }
}