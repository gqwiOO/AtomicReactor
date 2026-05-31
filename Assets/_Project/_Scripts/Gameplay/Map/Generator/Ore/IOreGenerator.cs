using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Generating.Ore
{
    public interface IOreGenerator
    {
        void GenerateOrePositions(Vector2Int mapSize, IBiomesService biomesService);
        CellType? GetOreCellType(Vector2Int position);
        void Clear();
    }
}
