using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Debugging;
using Core.Scripts.FastNoise;
using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Cell;
using Gameplay.Map.Cell.Data;
using Gameplay.Map.CellsService;
using SharpVoronoiLib;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Creator
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private CellsAssetCollectionDictionary cellsCollection;
        
        [SerializeField] private Transform _cellsParent;
        
        [SerializeField] private Vector2Int _mapSize;
        
        [Header("Water generating settings")] 
        [SerializeField]
        private float waterFrequency;
        
        [SerializeField]
        private float waterMaxValue;
        
        private readonly Dictionary<Vector2Int, ICell> _cellsDictionary = new Dictionary<Vector2Int, ICell>();
        private readonly Vector2Int[] _neighborOffsets =
        {
            new Vector2Int(1, 0),  // Right
            new Vector2Int(-1, 0), // Left
            new Vector2Int(0, 1),  // Up
            new Vector2Int(0, -1)  // Down
        };

        private IMapCellsService _mapCellsService;
        private IBiomesService _biomesService;
        private DiContainer _diContainer;
        private MapDataGenerator _mapDataGenerator;
        private bool _isInited;

        [Inject]
        private void Construct(IMapCellsService mapCellsService, DiContainer diContainer, IBiomesService biomesService)
        {
            _diContainer = diContainer;
            _mapCellsService = mapCellsService;
            _biomesService = biomesService;
        }

        private void OnDrawGizmos()
        {
            if (!_isInited)
                return;
            int cellX = 5;
            int cellY = 5;
            
            foreach (VoronoiEdge edge in _mapDataGenerator.BiomeEdges)
            {
                Vector2 start = new Vector2((float)edge.Start.X + 15, (float)edge.Start.Y + 15);
                Vector2 end = new Vector2((float)edge.End.X + 15, (float)edge.End.Y + 15);
                
                Gizmos.DrawLine(start, end);
            }
        }

        private VoronoiSite GetClosestVoronoiSite(int x, int y)
        {
            VoronoiSite nearestSite = _mapDataGenerator.BiomePlane.Sites
                .OrderBy(site => Math.Sqrt((site.X - x) * (site.X - x) + (site.Y - y) * (site.Y - y)))
                .First();
            return nearestSite;
        }

        private void Start()
        {
            _mapDataGenerator = new MapDataGenerator(_mapSize.x, _mapSize.y);
            _mapDataGenerator.SetWaterMaxValue(waterMaxValue); 
            _mapDataGenerator.WaterNoise.SetFrequency(waterFrequency);
            _mapDataGenerator.GenerateMap();
            CellType[,] mapуData = _mapDataGenerator.Map;

            _biomesService.SetVoronoiPlane(_mapDataGenerator.BiomePlane);
            
            FastNoise noise = new  FastNoise(UnityEngine.Random.Range(0, 9999));

            for (int y = 0; y < _mapSize.y; y++)
            {
                for (int x = 0; x < _mapSize.x; x++)
                {
                    CellComponent instance = null;
                    var biomeType = _biomesService.GetCellBiome(x, y);
                    
                    if (biomeType == BiomeType.Field || biomeType == BiomeType.Forest)
                    {
                        instance = _diContainer.InstantiatePrefabForComponent<CellComponent>(cellsCollection.GetCellPrefab(CellType.Grass), _cellsParent);
                    }
                    else
                    {
                        instance = _diContainer.InstantiatePrefabForComponent<CellComponent>(cellsCollection.GetCellPrefab(CellType.Water), _cellsParent);
                    }
                    Vector2Int position = new Vector2Int(x,y);
                    instance.SetPosition(position);
                    instance.transform.position = new Vector3(position.x,0,position.y);
                    _cellsDictionary.TryAdd(position, instance);
                    instance.RefreshMaterial();
                
                    instance.OnUpdated += Instance_OnUpdated;

                    instance.gameObject.name = $"Grid_Cell {position.x} {position.y}";
                }
            }

            _mapCellsService.InitCells(_cellsDictionary, _mapSize);
            _isInited = true;
        }

        private void Instance_OnUpdated(ICell cell)
        {
            foreach (var offset in _neighborOffsets)
            {
                Vector2Int neighborPosition = cell.Position + offset;
                if (_cellsDictionary.TryGetValue(neighborPosition, out ICell neighbor))
                {
                    Debugging.Log(this, $"Updated: {cell.Position}, Neighbor updated : {neighbor.Position}");
                    // neighbor.NotifyAboutNeighbourUpdated(cell);
                }
            }
        }
    }
}
