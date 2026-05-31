using Gameplay.Crafting;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Map.Building.Items.Data
{
    [CreateAssetMenu(menuName = "Core/Items/ItemDataAsset", fileName = "ItemDataAsset")]
    public class ItemDataAsset: ScriptableObject, IItemData, ICraftItem
    {
        [field:SerializeField]
        public int ItemId { get; set; }

        public int Amount => 1;

        [field:SerializeField]
        public string ItemName { get; set; }
        [field:SerializeField]
        public ItemMapObject ItemMapObjectPrefab { get; set; }
        [field:SerializeField]
        public Sprite ItemSprite { get; set;}
        
        [Header("Crafting")]
        [field: SerializeField]
        public bool IsCraftable { get; private set; }
        
        [field: ShowIf(nameof(IsCraftable))]
        [field: SerializeField]
        public CraftData CraftData { get; private set; }
    }
}