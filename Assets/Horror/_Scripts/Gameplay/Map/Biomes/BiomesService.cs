using System;
using System.Linq;
using Gameplay.Map.Creator;
using SharpVoronoiLib;

namespace GameAssembly.Horror._Scripts.Gameplay.Map.Biomes
{
    public class BiomesService : IBiomesService
    {
        private VoronoiPlane _voronoiPlane;
        
        public void SetVoronoiPlane(VoronoiPlane voronoiPlane)
        {
            _voronoiPlane = voronoiPlane;
        }

        public BiomeType GetCellBiome(int x, int y)
        {
            if (_voronoiPlane == null)
            {
                return BiomeType.None;
            }

            var site = GetClosestVoronoiSite(x, y);

            return site != null ? (BiomeType)site.Key : BiomeType.None;
        }
        private VoronoiSite GetClosestVoronoiSite(int x, int y)
        {
            VoronoiSite nearestSite = _voronoiPlane.Sites
                .OrderBy(site => Math.Sqrt((site.X - x) * (site.X - x) + (site.Y - y) * (site.Y - y)))
                .First();
            return nearestSite;
        }
    }
}