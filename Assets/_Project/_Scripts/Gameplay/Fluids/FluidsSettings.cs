using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public static class FluidsSettings
    {
        private static List<FluidData> _fluidsSettings;

        [InitializeOnEnterPlayMode]
        private static void Initialize()
        {
            _fluidsSettings = Resources.LoadAll<FluidData>("").ToList();
            Debug.Log($"Loaded {_fluidsSettings.Count} fluids");
        }

        public static FluidData GetFluid(FluidType type)
        {
            return _fluidsSettings.First(f => f.Name == type.ToString());
        }
        
        public static Color GetFluidColor(FluidType type)
        {
            return GetFluid(type).Color;
        }
        
        public static string GetFluidName(FluidType type)
        {
            return GetFluid(type).Name;
        }
    }
}