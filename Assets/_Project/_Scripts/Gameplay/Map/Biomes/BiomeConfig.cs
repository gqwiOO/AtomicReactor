using Gameplay.Map.Cell;
using Gameplay.Map.Creator;
using Gameplay.Map.Generating.Ore;
using UnityEngine;

namespace GameAssembly.Horror._Scripts.Gameplay.Map.Biomes
{
    [CreateAssetMenu(fileName = "BiomeConfig", menuName = "Configs/BiomeConfig")]
    public class BiomeConfig : ScriptableObject
    {
        [Header("Identity")]
        public BiomeType BiomeType;

        [Header("Generation")]
        [Min(0f)] public float SpawnWeight = 1f;

        [Header("Ground")]
        public CellType GroundCellType;

        [Header("Ore Spawning")]
        public OreGenerationConfig[] OreConfigs;

        [Header("Tree Spawning")]
        public bool CanSpawnTrees;
        [Range(0.001f, 1f)] public float TreeNoiseFrequency = 0.2f;
        [Range(0f, 1f)] public float TreeDensity = 0.4f;
    }
}
