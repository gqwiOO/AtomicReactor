using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Debugging;
using Cysharp.Threading.Tasks;
using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Cell;
using Gameplay.Map.Cell.Data;
using Gameplay.Map.Chunk;
using Gameplay.Map.CellsService;
using Gameplay.Map.Generating.Ore;
using Gameplay.Map.Generating.Wood;
using SharpVoronoiLib;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Creator
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private CellsAssetCollectionDictionary cellsCollection;
        [SerializeField] private BiomesCollection _biomesCollection;
        [SerializeField] private Transform _cellsParent;
        [SerializeField] private Vector2Int _mapSize;

        [Header("Water generating settings")]
        [SerializeField] private float waterFrequency;
        [SerializeField] private float waterMaxValue;

        private readonly Dictionary<Vector2Int, ICell> _cellsDictionary = new Dictionary<Vector2Int, ICell>();
        private readonly Vector2Int[] _neighborOffsets =
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        private IMapCellsService _mapCellsService;
        private IBiomesService _biomesService;
        private IWoodGenerator _woodGenerator;
        private IOreGenerator _oreGenerator;
        private IMapChunkService _chunkService;
        private DiContainer _diContainer;
        private MapDataGenerator _mapDataGenerator;
        private ChunkBorderRenderer _chunkBorderRenderer;
        private bool _isInited;

        [Inject]
        private void Construct(IMapCellsService mapCellsService, DiContainer diContainer, IBiomesService biomesService,
            IWoodGenerator woodGenerator, IOreGenerator oreGenerator, IMapChunkService chunkService)
        {
            _diContainer = diContainer;
            _mapCellsService = mapCellsService;
            _biomesService = biomesService;
            _woodGenerator = woodGenerator;
            _oreGenerator = oreGenerator;
            _chunkService = chunkService;
        }

        private void OnDrawGizmos()
        {
            if (!_isInited)
                return;

            foreach (VoronoiEdge edge in _mapDataGenerator.BiomeEdges)
            {
                Vector2 start = new Vector2((float)edge.Start.X + 15, (float)edge.Start.Y + 15);
                Vector2 end = new Vector2((float)edge.End.X + 15, (float)edge.End.Y + 15);
                Gizmos.DrawLine(start, end);
            }
        }

        private void Start()
        {
            var borderGo = new GameObject("ChunkBorderRenderer");
            _chunkBorderRenderer = _diContainer.InstantiateComponent<ChunkBorderRenderer>(borderGo);
            BuildMap(waterMaxValue, waterFrequency);
        }

        public void RegenerateWithForcedBiome(BiomeType biome)
        {
            _woodGenerator.Clear();
            _oreGenerator.Clear();
            _chunkService.Clear();

            foreach (Transform child in _cellsParent)
                Destroy(child.gameObject);
            _cellsDictionary.Clear();

            _biomesService.ForceBiome(biome);
            BuildMap(noWater: true);
            _biomesService.ForceBiome(null);

            _woodGenerator.GenerateWoods().Forget();
        }

        private void BuildMap(float overrideWaterMaxValue = -1f, float overrideWaterFrequency = -1f, bool noWater = false)
        {
            _mapDataGenerator = new MapDataGenerator(_mapSize.x, _mapSize.y, _biomesCollection);

            float wMax = noWater ? 2f : (overrideWaterMaxValue >= 0 ? overrideWaterMaxValue : waterMaxValue);
            float wFreq = overrideWaterFrequency >= 0 ? overrideWaterFrequency : waterFrequency;

            _mapDataGenerator.SetWaterMaxValue(wMax);
            _mapDataGenerator.WaterNoise.SetFrequency(wFreq);
            _mapDataGenerator.GenerateMap();

            _biomesService.SetVoronoiPlane(_mapDataGenerator.BiomePlane, _biomesCollection.EdgeNoiseScale, _biomesCollection.EdgeNoiseFrequency);
            _oreGenerator.GenerateOrePositions(_mapSize, _biomesService);

            SpawnCells();

            _mapCellsService.InitCells(_cellsDictionary, _mapSize);
            _chunkService.BuildChunks(_cellsDictionary, _cellsParent);
            _chunkBorderRenderer.RebuildBorders();
            _isInited = true;
        }

        private void SpawnCells()
        {
            for (int y = 0; y < _mapSize.y; y++)
            {
                for (int x = 0; x < _mapSize.x; x++)
                {
                    var biomeType = _biomesService.GetCellBiome(x, y);
                    var position = new Vector2Int(x, y);
                    CellType cellType = BiomeToCellType(biomeType, position);

                    CellComponent instance = _diContainer.InstantiatePrefabForComponent<CellComponent>(
                        cellsCollection.GetCellPrefab(cellType), _cellsParent);

                    instance.SetPosition(position);
                    instance.transform.position = new Vector3(position.x, 0, position.y);
                    _cellsDictionary.TryAdd(position, instance);
                    instance.RefreshMaterial();
                    instance.OnUpdated += Instance_OnUpdated;
                    instance.gameObject.name = $"Grid_Cell {position.x} {position.y}";
                }
            }
        }

        private CellType BiomeToCellType(BiomeType biome, Vector2Int position)
        {
            var oreCellType = _oreGenerator.GetOreCellType(position);
            if (oreCellType.HasValue)
                return oreCellType.Value;

            var config = _biomesCollection.GetConfig(biome);
            return config?.GroundCellType ?? CellType.Grass;
        }

        private void Instance_OnUpdated(ICell cell)
        {
            foreach (var offset in _neighborOffsets)
            {
                Vector2Int neighborPosition = cell.Position + offset;
                if (_cellsDictionary.TryGetValue(neighborPosition, out ICell neighbor))
                {
                    Debugging.Log(this, $"Updated: {cell.Position}, Neighbor updated : {neighbor.Position}");
                }
            }
        }
    }
}
