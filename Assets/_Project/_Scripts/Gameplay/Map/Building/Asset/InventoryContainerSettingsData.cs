using System;
using Gameplay.Map.Building.Furnace;

namespace Gameplay.Map.Building
{
    [Serializable]
    public class InventoryContainerSettingsData: BuildingSettingsData
    {
        public int CellCapacity;
        public int InventoryCapacity;
    }
}