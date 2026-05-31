using System.Collections.Generic;
using System.Linq;
using Gameplay.Items;
using ModestTree;
using UnityEngine;

namespace Gameplay.Crafting
{
    public class CraftService: MonoBehaviour, ICraftService
    {
        [field: SerializeField] public ItemsAssetCollection ItemsAssetCollection { get; private set; }
        private List<CraftData> _craftRecipes;

        private List<CraftData> CraftRecipes
        {
            get
            {
                if (_craftRecipes == null || _craftRecipes.IsEmpty())
                {
                    _craftRecipes = ItemsAssetCollection.Assets
                        .Where(x => x.IsCraftable)
                        .Select(item => item.CraftData).ToList();
                }

                return _craftRecipes;
            }
        }

        public List<CraftData> GetAllItemsCrafts(int itemId)
        {
            return CraftRecipes.Where(c => c.Outputs.Any(o => o.ItemId == itemId)).ToList();
        }

        public CraftData GetItemsCraft(int itemId)
        {
            return CraftRecipes.First(c => c.Outputs.Any(o => o.ItemId == itemId));
        }
    }

    public interface ICraftService
    {
        List<CraftData> GetAllItemsCrafts(int itemId);
        CraftData GetItemsCraft(int itemId);
    }
}