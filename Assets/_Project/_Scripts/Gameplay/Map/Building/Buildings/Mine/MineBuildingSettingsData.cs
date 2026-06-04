using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building.Furnace;
using Gameplay.Map.Building.Items.Data;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Mine
{
    [Serializable]
    public class MineBuildingSettingsData : BuildingSettingsData
    {
        public float ElectricityPerExtraction;
        public float MiningInterval;
        public int StorageCellCapacity;

        [field: SerializeField]
        public List<CellTypeToItemMapping> OreItemMappings { get; set; } = new();

        public int GetItemIdForCellType(CellType cellType)
        {
            return OreItemMappings.FirstOrDefault(m => m.CellType == cellType)?.Item?.ItemId ?? -1;
        }
    }

    [Serializable]
    public class CellTypeToItemMapping
    {
        public CellType CellType;
        public ItemDataAsset Item;
    }
}
