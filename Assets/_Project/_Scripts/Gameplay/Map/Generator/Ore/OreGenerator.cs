using System.Collections.Generic;
using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Cell;
using Gameplay.Map.Creator;
using UnityEngine;

namespace Gameplay.Map.Generating.Ore
{
    public class OreGenerator : MonoBehaviour, IOreGenerator
    {
        [SerializeField] private BiomesCollection _biomesCollection;

        private readonly Dictionary<Vector2Int, CellType> _orePositions = new Dictionary<Vector2Int, CellType>();

        private readonly Vector2Int[] _neighborOffsets =
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        public void Clear() => _orePositions.Clear();

        public CellType? GetOreCellType(Vector2Int position) =>
            _orePositions.TryGetValue(position, out var cellType) ? cellType : (CellType?)null;

        public void GenerateOrePositions(Vector2Int mapSize, IBiomesService biomesService)
        {
            _orePositions.Clear();

            foreach (var biomeConfig in _biomesCollection.Biomes)
                foreach (var oreConfig in biomeConfig.OreConfigs)
                    GenerateForBiome(biomeConfig.BiomeType, oreConfig, mapSize, biomesService);
        }

        private void GenerateForBiome(BiomeType biomeType, OreGenerationConfig config, Vector2Int mapSize, IBiomesService biomesService)
        {
            var allowedPositions = new List<Vector2Int>();
            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                    if (biomesService.GetCellBiome(x, y) == biomeType)
                        allowedPositions.Add(new Vector2Int(x, y));

            Shuffle(allowedPositions);

            int seedCount = Mathf.CeilToInt(allowedPositions.Count * config.VeinDensity);

            for (int i = 0; i < seedCount && i < allowedPositions.Count; i++)
            {
                Vector2Int seed = allowedPositions[i];
                if (_orePositions.ContainsKey(seed))
                    continue;

                int veinSize = Random.Range(5, config.MaxVeinSize + 1);
                var vein = new List<Vector2Int> { seed };
                var veinSet = new HashSet<Vector2Int> { seed };

                int maxAttempts = veinSize * 10;
                int attempts = 0;

                while (vein.Count < veinSize && attempts < maxAttempts)
                {
                    attempts++;
                    Vector2Int origin = vein[Random.Range(0, vein.Count)];
                    Vector2Int candidate = origin + _neighborOffsets[Random.Range(0, _neighborOffsets.Length)];

                    if (veinSet.Contains(candidate) || _orePositions.ContainsKey(candidate))
                        continue;
                    if (candidate.x < 0 || candidate.x >= mapSize.x || candidate.y < 0 || candidate.y >= mapSize.y)
                        continue;
                    if (biomesService.GetCellBiome(candidate.x, candidate.y) != biomeType)
                        continue;

                    vein.Add(candidate);
                    veinSet.Add(candidate);
                }

                foreach (Vector2Int pos in vein)
                    _orePositions[pos] = config.OreType;
            }
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
