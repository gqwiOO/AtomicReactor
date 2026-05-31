using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Generating.Ore
{
    [CreateAssetMenu(fileName = "OreGenerationConfig", menuName = "Configs/OreGenerationConfig")]
    public class OreGenerationConfig : ScriptableObject
    {
        public CellType OreType;
        [Min(1)] public int MaxVeinSize = 25;
        [Range(0f, 1f)] public float VeinDensity = 0.04f;
    }
}
