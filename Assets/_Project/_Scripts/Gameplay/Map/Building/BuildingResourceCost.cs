using System;
using Gameplay.Map.Building.Items.Data;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [Serializable]
    public class BuildingResourceCost
    {
        [field: SerializeField] public ItemDataAsset Item { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}
