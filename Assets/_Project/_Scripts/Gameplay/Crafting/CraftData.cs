using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building.Items.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Crafting
{
    [Serializable]
    public class CraftData
    {
        public List<ItemDataAsset> InputAssets;
        public List<ItemDataAsset> OutputAssets;
        
        private List<CraftItem> _inputs;
        public IEnumerable<ICraftItem> Inputs => InputAssets.Select(i => i as ICraftItem).ToList();
        public IEnumerable<ICraftItem> Outputs => OutputAssets.Select(i => i as ICraftItem).ToList();
        
    }

    [Serializable]
    public class CraftItem: ICraftItem
    {
        [field:SerializeField]
        public int ItemId{ get; set;}
        
        [field:SerializeField]
        public int Amount { get; set; }

        public CraftItem(int itemId, int amount)
        {
            ItemId = itemId;
            Amount = amount;
        }
    }

    public interface ICraftItem
    {
        public int ItemId { get; }
        public int Amount { get; }
    }
}