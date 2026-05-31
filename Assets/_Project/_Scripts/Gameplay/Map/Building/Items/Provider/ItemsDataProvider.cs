using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building.Items.Data;
using UnityEngine;

namespace Gameplay.Map.Building.Items.Provider
{
    public class ItemsDataProvider : MonoBehaviour, IItemsDataProvider
    {
        [SerializeField] 
        private List<ItemDataAsset> _itemDataAssets;
        
        public Sprite GetItemSprite(int id)
        {
            return _itemDataAssets.FirstOrDefault(i => i.ItemId == id)?.ItemSprite;
        }

        public IEnumerable<IItemData> GetAllItems()
        {
            return _itemDataAssets;
        }
    }
}