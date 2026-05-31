using UnityEngine;

namespace Gameplay.Map.Chunk
{
    public class MapChunk
    {
        public const int Size = 16;

        public Vector2Int ChunkCoord { get; }
        public GameObject Root { get; }

        public MapChunk(Vector2Int chunkCoord, GameObject root)
        {
            ChunkCoord = chunkCoord;
            Root = root;
        }
    }
}
