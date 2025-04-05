using System;
using System.Collections.Generic;
using Core.Scripts.Extension.System;
using Core.Scripts.FastNoise;
using Gameplay.Map.Cell;
using SharpVoronoiLib;

namespace Gameplay.Map.Creator
{
    public class MapDataGenerator
    {

        private int _width;
        private int _height;
        private float _waterMaxValue;
        
        public List<VoronoiEdge> BiomeEdges;

        public VoronoiPlane BiomePlane { get; private set; }

        public CellType[,] Map { get; private set; }
        public FastNoise WaterNoise { get; private set; }
        public bool[,] WaterMap { get; private set; }
        
        public MapDataGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            Map = new CellType[width, height];
            WaterMap = new bool[width, height];
            
            WaterNoise = new FastNoise(UnityEngine.Random.Range(0, 9999));
        }
        public void GenerateMap()
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    WaterMap[x, y] = WaterNoise.GetValue(x,y) > _waterMaxValue;
                }
            }

            GenerateBiomeMap();

            MergeMaps();
        }
        
        private void GenerateBiomeMap()
        {
            int numPoints = _width * _height / 100;
        
            List<VoronoiSite> sites = new List<VoronoiSite>(numPoints);

            int seed = new Random().Next();

            Random rand = new Random(seed);
            for (int i = 0; i < numPoints; i++)
            {
                sites.Add(
                    new VoronoiSite(
                        rand.Next(_width), 
                        rand.Next(_height),
                        (int)EnumExtension.GetRandomExceptZero<BiomeType>()
                    )
                );
            }

            int duplicates = rand.Next(numPoints / 20);
        
            for (int i = 0; i < duplicates; i++)
            {
                int i1 = rand.Next(numPoints);
                int i2 = rand.Next(numPoints);
            
                // "Duplicate"
                sites[i1] = new VoronoiSite(sites[i2].X, sites[i2].Y);
            }

            BiomePlane = new VoronoiPlane(0, 0, _width, _height);
        
            BiomePlane.SetSites(sites);
            BiomePlane.Tessellate();
            BiomeEdges = BiomePlane.Relax();
        }

        public void SetWaterMaxValue(float value)
        {
            _waterMaxValue = value;
        }

        private void MergeMaps()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Map[y,x] = WaterMap[x, y] == true ? CellType.Water : CellType.Grass;   
                }
            }
        }
    }
}