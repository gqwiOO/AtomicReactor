using System.Collections.Generic;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Chunk
{
    public interface IMapChunkService
    {
        IReadOnlyDictionary<Vector2Int, MapChunk> Chunks { get; }
        void BuildChunks(Dictionary<Vector2Int, ICell> cells, Transform parent);
        void Clear();
    }
}
