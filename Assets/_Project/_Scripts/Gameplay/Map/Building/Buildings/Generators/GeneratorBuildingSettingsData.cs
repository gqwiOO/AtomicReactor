using System;
using Gameplay.Map.Building.Furnace;

namespace Gameplay.Map.Building.Generators
{
    [Serializable]
    public class GeneratorBuildingSettingsData: BuildingSettingsData
    {
        public float Power;
        public float Ah_BatteryCapacity;

        public BuildingSidesData BuildingSidesData;
    }
}