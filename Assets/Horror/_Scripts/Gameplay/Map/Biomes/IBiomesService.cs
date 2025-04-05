using Gameplay.Map.CellsService;
using Gameplay.Map.Creator;
using SharpVoronoiLib;
using UnityEngine;
using Zenject;

namespace GameAssembly.Horror._Scripts.Gameplay.Map.Biomes
{
    public interface IBiomesService
    {
        BiomeType GetCellBiome(int x, int y);
        void SetVoronoiPlane(VoronoiPlane voronoiPlane);
    }
}