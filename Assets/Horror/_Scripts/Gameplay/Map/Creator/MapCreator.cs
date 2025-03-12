using System.Collections.Generic;
using Core.Scripts.Debugging;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Creator
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private CellComponent _cellPrefab;

        [SerializeField] private Transform _cellsParent;
        
        [SerializeField] private int _cellsCount;

        [SerializeField] private Vector2Int _mapSize;

        private readonly Dictionary<Vector2Int, ICell> _cellsDictionary = new Dictionary<Vector2Int, ICell>();
        private readonly Vector2Int[] _neighborOffsets =
        {
            new Vector2Int(1, 0),  // Right
            new Vector2Int(-1, 0), // Left
            new Vector2Int(0, 1),  // Up
            new Vector2Int(0, -1)  // Down
        };

        private IMapCellsService _mapCellsService;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(IMapCellsService mapCellsService, DiContainer diContainer)
        {
            _diContainer = diContainer;
            _mapCellsService = mapCellsService;
        }

        private void Start()
        {
            for (int i = 0; i < _cellsCount; i++)
            {
                var instance = _diContainer.InstantiatePrefabForComponent<CellComponent>(_cellPrefab, _cellsParent);
                Vector2Int position = new Vector2Int(i % _mapSize.x, i / _mapSize.x);
                instance.SetPosition(position);
                _cellsDictionary.TryAdd(position, instance);
                instance.RefreshMaterial();
                
                instance.OnUpdated += Instance_OnUpdated;

                instance.gameObject.name = $"Grid_Cell {position.x} {position.y}";
            }

            _mapCellsService.InitCells(_cellsDictionary, _mapSize);
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
