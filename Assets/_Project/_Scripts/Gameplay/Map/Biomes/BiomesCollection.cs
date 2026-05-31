using Gameplay.Map.Creator;
using UnityEngine;

namespace GameAssembly.Horror._Scripts.Gameplay.Map.Biomes
{
    [CreateAssetMenu(fileName = "BiomesCollection", menuName = "Configs/BiomesCollection")]
    public class BiomesCollection : ScriptableObject
    {
        [Min(1)] public int SiteDensityDivisor = 1500;
        [Min(1)] public int RelaxPasses = 5;

        [Header("Edge Noise")]
        [Min(0f)] public float EdgeNoiseScale = 20f;
        [Range(0.001f, 1f)] public float EdgeNoiseFrequency = 0.03f;

        public BiomeConfig[] Biomes;

        public BiomeConfig GetConfig(BiomeType biomeType)
        {
            foreach (var config in Biomes)
                if (config.BiomeType == biomeType)
                    return config;
            return null;
        }
    }
}
