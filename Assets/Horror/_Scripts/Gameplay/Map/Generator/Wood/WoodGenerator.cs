using Core.Scripts.Debugging;
using Core.Scripts.FastNoise;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using Gameplay.Map.Nature;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Generating.Wood
{
    public class WoodGenerator : MonoBehaviour, IWoodGenerator
    {
        private IMapCellsService _mapCellsService;
        [SerializeField] private float frequency;
        [SerializeField] private float FractalLacunarity;
        
        [SerializeField]
        private TreeMapObject treePrefab;
        

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }


        private async UniTask  Start()
        {
            await UniTask.Delay(500);
            
            GenerateWoods().Forget();
        }

        public async UniTask GenerateWoods()
        {
            FastNoise fastNoise = new FastNoise();
            fastNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
            fastNoise.SetFrequency(frequency);
            fastNoise.SetInterp(FastNoise.Interp.Quintic);
            
            Vector2Int mapSize = _mapCellsService.MapSize;

            bool[,] treeMap = new bool[mapSize.y, mapSize.x];
            
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int x = 0; x < mapSize.x; x++)
                {
                    float value =  fastNoise.GetValue(x, y);
                    if (value < 0)
                    {
                        treeMap[y, x] = true;
                        ICell cell = _mapCellsService.GetCell(new Vector2Int(x, y));
                        TreeMapObject tree = Instantiate(treePrefab, cell.WorldPosition, Quaternion.identity);
                        cell.SetVisitor(tree);
                    }
                    Debugging.Log(this,$"{value}");
                }
            }
        }
    }
}