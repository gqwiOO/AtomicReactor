using System.Collections.Generic;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Chunk
{
    public class MapChunkService : IMapChunkService
    {
        private readonly Dictionary<Vector2Int, MapChunk> _chunks = new();
        public IReadOnlyDictionary<Vector2Int, MapChunk> Chunks => _chunks;

        public void BuildChunks(Dictionary<Vector2Int, ICell> cells, Transform parent)
        {
            foreach (var (pos, cell) in cells)
            {
                var chunkCoord = new Vector2Int(pos.x / MapChunk.Size, pos.y / MapChunk.Size);
                if (!_chunks.TryGetValue(chunkCoord, out var chunk))
                {
                    var root = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
                    root.transform.SetParent(parent, false);
                    chunk = new MapChunk(chunkCoord, root);
                    _chunks[chunkCoord] = chunk;
                }

                var cellMb = cell as MonoBehaviour;
                if (cellMb != null)
                    cellMb.transform.SetParent(chunk.Root.transform, true);
            }

            foreach (var chunk in _chunks.Values)
                StaticBatchingUtility.Combine(chunk.Root);
        }

        public void Clear() => _chunks.Clear();
    }
}
