using System;
using System.Linq;
using Core.Scripts.FastNoise;
using Gameplay.Map.Creator;
using SharpVoronoiLib;

namespace GameAssembly.Horror._Scripts.Gameplay.Map.Biomes
{
    public class BiomesService : IBiomesService
    {
        private VoronoiPlane _voronoiPlane;
        private BiomeType? _forcedBiome;

        private FastNoise _noiseX;
        private FastNoise _noiseY;
        private float _edgeNoiseScale;

        public void SetVoronoiPlane(VoronoiPlane voronoiPlane, float edgeNoiseScale, float edgeNoiseFrequency)
        {
            _voronoiPlane = voronoiPlane;
            _edgeNoiseScale = edgeNoiseScale;

            _noiseX = new FastNoise(1337);
            _noiseX.SetNoiseType(FastNoise.NoiseType.Perlin);
            _noiseX.SetFrequency(edgeNoiseFrequency);

            _noiseY = new FastNoise(7331);
            _noiseY.SetNoiseType(FastNoise.NoiseType.Perlin);
            _noiseY.SetFrequency(edgeNoiseFrequency);
        }

        public void ForceBiome(BiomeType? biome)
        {
            _forcedBiome = biome;
        }

        public BiomeType GetCellBiome(int x, int y)
        {
            if (_forcedBiome.HasValue)
                return _forcedBiome.Value;

            if (_voronoiPlane == null)
                return BiomeType.None;

            float wx = x + _noiseX.GetValue(x, y) * _edgeNoiseScale;
            float wy = y + _noiseY.GetValue(x, y) * _edgeNoiseScale;

            var site = GetClosestVoronoiSite(wx, wy);
            return site != null ? (BiomeType)site.Key : BiomeType.None;
        }

        private VoronoiSite GetClosestVoronoiSite(float x, float y)
        {
            return _voronoiPlane.Sites
                .OrderBy(site => (site.X - x) * (site.X - x) + (site.Y - y) * (site.Y - y))
                .First();
        }
    }
}
