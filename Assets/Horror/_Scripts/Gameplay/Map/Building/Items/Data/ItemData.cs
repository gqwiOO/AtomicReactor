using System;
using UnityEngine;

namespace Gameplay.Map.Building.Items.Data
{
    [Serializable]
    public class ItemData : IItemData
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public ItemMapObject ItemMapObjectPrefab{ get; set; }
        public Sprite ItemSprite{ get; set; }

        public ItemData(ItemDataAsset asset)
        {
            ItemId = asset.ItemId;
            ItemName = asset.ItemName;
            ItemMapObjectPrefab = asset.ItemMapObjectPrefab;
            ItemSprite = asset.ItemSprite;
        }
        
    }

    public class ItemMapObject : MonoBehaviour
    {
    }

    public interface IItemData
    {
        public int ItemId { get; }
        public string ItemName { get; }
    }
}