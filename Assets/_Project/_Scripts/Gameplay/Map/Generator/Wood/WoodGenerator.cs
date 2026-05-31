using Core.Scripts.FastNoise;
using Cysharp.Threading.Tasks;
using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using Gameplay.Map.Creator;
using Gameplay.Map.Nature;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Generating.Wood
{
    public class WoodGenerator : MonoBehaviour, IWoodGenerator
    {
        [SerializeField] private BiomesCollection _biomesCollection;
        [SerializeField] private TreeMapObject treePrefab;
        [SerializeField] private Transform _treesParent;

        private IMapCellsService _mapCellsService;
        private IBiomesService _biomesService;

        [Inject]
        private void Construct(IMapCellsService mapCellsService, IBiomesService biomesService)
        {
            _mapCellsService = mapCellsService;
            _biomesService = biomesService;
        }

        private async UniTask Start()
        {
            await UniTask.Delay(500);
            GenerateWoods().Forget();
        }

        public void Clear()
        {
            foreach (Transform child in _treesParent)
                Destroy(child.gameObject);
        }

        public async UniTask GenerateWoods()
        {
            if (_biomesCollection == null)
            {
                UnityEngine.Debug.LogError("[WoodGenerator] _biomesCollection is not assigned in the Inspector.");
                return;
            }

            Vector2Int mapSize = _mapCellsService.MapSize;

            foreach (var biomeConfig in _biomesCollection.Biomes)
            {
                if (!biomeConfig.CanSpawnTrees)
                    continue;

                var noise = new FastNoise();
                noise.SetNoiseType(FastNoise.NoiseType.Perlin);
                noise.SetFrequency(biomeConfig.TreeNoiseFrequency);
                noise.SetInterp(FastNoise.Interp.Quintic);

                for (int y = 0; y < mapSize.y; y++)
                {
                    for (int x = 0; x < mapSize.x; x++)
                    {
                        if (_biomesService.GetCellBiome(x, y) != biomeConfig.BiomeType)
                            continue;

                        float noiseValue = (noise.GetValue(x, y) + 1f) * 0.5f;
                        if (Random.value > noiseValue * biomeConfig.TreeDensity)
                            continue;

                        ICell cell = _mapCellsService.GetCell(new Vector2Int(x, y));
                        if (cell.CellType != biomeConfig.GroundCellType || cell.CellVisitor != null)
                            continue;

                        TreeMapObject tree = Instantiate(treePrefab, cell.WorldPosition, Quaternion.identity, _treesParent);
                        cell.SetVisitor(tree);
                    }
                }
            }

            StaticBatchingUtility.Combine(_treesParent.gameObject);
        }
    }
}
