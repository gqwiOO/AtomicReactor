using System;
using System.Collections.Generic;
using Core.Scripts.FastNoise;
using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Cell;
using SharpVoronoiLib;

namespace Gameplay.Map.Creator
{
    public class MapDataGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly BiomesCollection _biomesCollection;
        private float _waterMaxValue;

        public List<VoronoiEdge> BiomeEdges;
        public VoronoiPlane BiomePlane { get; private set; }
        public CellType[,] Map { get; private set; }
        public FastNoise WaterNoise { get; private set; }
        public bool[,] WaterMap { get; private set; }

        public MapDataGenerator(int width, int height, BiomesCollection biomesCollection)
        {
            _width = width;
            _height = height;
            _biomesCollection = biomesCollection;
            Map = new CellType[width, height];
            WaterMap = new bool[width, height];
            WaterNoise = new FastNoise(UnityEngine.Random.Range(0, 9999));
        }

        public void GenerateMap()
        {
            for (int x = 0; x < Map.GetLength(0); x++)
                for (int y = 0; y < Map.GetLength(1); y++)
                    WaterMap[x, y] = WaterNoise.GetValue(x, y) > _waterMaxValue;

            GenerateBiomeMap();
            MergeMaps();
        }

        private void GenerateBiomeMap()
        {
            int numPoints = _width * _height / _biomesCollection.SiteDensityDivisor;

            var sites = new List<VoronoiSite>(numPoints);
            var rand = new Random();

            for (int i = 0; i < numPoints; i++)
            {
                BiomeType biome = GetWeightedRandomBiome(rand);
                sites.Add(new VoronoiSite(rand.Next(_width), rand.Next(_height), (int)biome));
            }

            BiomePlane = new VoronoiPlane(0, 0, _width, _height);
            BiomePlane.SetSites(sites);
            BiomePlane.Tessellate();
            for (int i = 0; i < _biomesCollection.RelaxPasses; i++)
                BiomeEdges = BiomePlane.Relax();
        }

        public void SetWaterMaxValue(float value)
        {
            _waterMaxValue = value;
        }

        private void MergeMaps()
        {
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    Map[y, x] = WaterMap[x, y] ? CellType.Water : CellType.Grass;
        }

        private BiomeType GetWeightedRandomBiome(Random rand)
        {
            float totalWeight = 0f;
            foreach (var config in _biomesCollection.Biomes)
                totalWeight += config.SpawnWeight;

            float roll = (float)(rand.NextDouble() * totalWeight);
            float cumulative = 0f;
            foreach (var config in _biomesCollection.Biomes)
            {
                cumulative += config.SpawnWeight;
                if (roll < cumulative)
                    return config.BiomeType;
            }

            return _biomesCollection.Biomes[0].BiomeType;
        }
    }
}
