using Gameplay.Map.Building.Items.Data;
using Mechanics.Data;
using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(menuName = "Core/Collections/ItemsAssetCollection", fileName = "ItemsAssetCollection")]
    public class ItemsAssetCollection : AssetCollection<ItemDataAsset>
    {
    }
}