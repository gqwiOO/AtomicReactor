using Gameplay.Map.Creator;
using SharpVoronoiLib;

namespace GameAssembly.Horror._Scripts.Gameplay.Map.Biomes
{
    public interface IBiomesService
    {
        BiomeType GetCellBiome(int x, int y);
        void SetVoronoiPlane(VoronoiPlane voronoiPlane, float edgeNoiseScale, float edgeNoiseFrequency);
        void ForceBiome(BiomeType? biome);
    }
}
