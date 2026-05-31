using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Chunk
{
    public class ChunkBorderRenderer : MonoBehaviour
    {
        private readonly List<GameObject> _borders = new();
        private bool _visible;
        private IMapChunkService _chunkService;
        private Material _material;

        [Inject]
        private void Construct(IMapChunkService chunkService)
        {
            _chunkService = chunkService;
        }

        public void RebuildBorders()
        {
            foreach (var go in _borders)
                if (go != null) Destroy(go);
            _borders.Clear();

            _material ??= CreateLineMaterial();

            foreach (var chunk in _chunkService.Chunks.Values)
            {
                var go = new GameObject($"Border_{chunk.ChunkCoord.x}_{chunk.ChunkCoord.y}");
                go.transform.SetParent(transform, false);

                var lr = go.AddComponent<LineRenderer>();
                lr.material = _material;
                lr.useWorldSpace = true;
                lr.loop = false;
                lr.startWidth = 0.12f;
                lr.endWidth = 0.12f;
                lr.positionCount = 5;
                lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                lr.receiveShadows = false;
                lr.generateLightingData = false;

                float x0 = chunk.ChunkCoord.x * MapChunk.Size;
                float z0 = chunk.ChunkCoord.y * MapChunk.Size;
                float x1 = x0 + MapChunk.Size;
                float z1 = z0 + MapChunk.Size;
                const float y = 0.55f;

                lr.SetPositions(new[]
                {
                    new Vector3(x0, y, z0),
                    new Vector3(x1, y, z0),
                    new Vector3(x1, y, z1),
                    new Vector3(x0, y, z1),
                    new Vector3(x0, y, z0),
                });

                go.SetActive(false);
                _borders.Add(go);
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.F3)) return;

            _visible = !_visible;
            foreach (var go in _borders)
                if (go != null) go.SetActive(_visible);
        }

        private static Material CreateLineMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                         ?? Shader.Find("Unlit/Color");
            var mat = new Material(shader);
            mat.SetColor("_BaseColor", new Color(1f, 0.85f, 0f));
            mat.SetColor("_Color", new Color(1f, 0.85f, 0f));
            mat.hideFlags = HideFlags.HideAndDontSave;
            return mat;
        }
    }
}
