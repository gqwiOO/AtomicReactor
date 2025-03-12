using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.CellsService
{
    public class MapCellsService : IMapCellsService
    {
        private Dictionary<Vector2Int, ICell> _cellsDictionary;

        private readonly Vector2Int[] _neighborOffsets =
        {
            new Vector2Int(1, 0),  // Right
            new Vector2Int(-1, 0), // Left
            new Vector2Int(0, 1),  // Up
            new Vector2Int(0, -1)  // Down
        };

        public Vector2Int MapSize { get; private set; }
        public ICell GetCell(Vector2Int cell)
        {
            _cellsDictionary.TryGetValue(cell, out ICell result);
            return result;
        }

        public void InitCells(Dictionary<Vector2Int, ICell> cells, Vector2Int mapSize)
        {
            _cellsDictionary = cells;
            MapSize = mapSize;
        }

        public ICell GetCellWithVisitor<TVisitor>(out TVisitor visitor) where TVisitor : class,ICellVisitor 
        {
            ICell result = _cellsDictionary.Values.FirstOrDefault(cell => cell.CellVisitor is TVisitor);
            visitor = result?.CellVisitor as TVisitor;
            return result;
        }

        public ICell GetClosestCellWithVisitor<TVisitor>(out TVisitor visitor, Vector2Int targetCell) where TVisitor : class, ICellVisitor
        {
            visitor = null;
            var visited = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();
            queue.Enqueue(targetCell);
            visited.Add(targetCell);

            while (queue.Count > 0)
            {
                var currentCellPosition = queue.Dequeue();
                if (_cellsDictionary.TryGetValue(currentCellPosition, out ICell currentCell) && currentCell.CellVisitor is TVisitor)
                {
                    visitor = currentCell.CellVisitor as TVisitor;
                    return currentCell;
                }

                foreach (var offset in _neighborOffsets)
                {
                    var neighborPosition = currentCellPosition + offset;
                    if (!visited.Contains(neighborPosition))
                    {
                        queue.Enqueue(neighborPosition);
                        visited.Add(neighborPosition);
                    }
                }
            }

            return null;
        }

        public ICell GetClosestEmptyCellWithNeighbourCellVisitor<TVisitor>(out TVisitor visitor, out ICell visitorCell, Vector2Int targetCell) where TVisitor : class, ICellVisitor
        {
            visitorCell = GetClosestCellWithVisitor<TVisitor>(out TVisitor cellVisitor, targetCell);
            visitor = cellVisitor;

            var closestEmptyCell = GetCellNeighbours(visitorCell.Position)
                .Where(neighbour => neighbour.Item1.CellVisitor == null)
                .OrderBy(neighbour => Vector2Int.Distance(neighbour.Item2, targetCell))
                .FirstOrDefault()?.Item1;

            return closestEmptyCell;
        }

        public async Task<ICell> GetCellWithVisitorAsync<TVisitor>() where TVisitor : class,ICellVisitor 
        {
            ICell result =  _cellsDictionary.Values.AsParallel().FirstOrDefault(cell => cell.CellVisitor is TVisitor)!;
            return result;
        }

        public IEnumerable<Tuple<ICell, Vector2Int>> GetCellNeighbours(Vector2Int position, int radius)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var neighbourPosition = position + new Vector2Int(dx,dy);
                    _cellsDictionary.TryGetValue(neighbourPosition, out ICell cell);
                    
                    if (cell != null)
                        yield return new Tuple<ICell, Vector2Int>(cell, neighbourPosition);
                }
            }
        }
        
        public IEnumerable<Tuple<ICell, Vector2Int>> GetCellNeighboursWithVisitor<TVisitor>(Vector2Int position, int radius) where TVisitor: ICellVisitor
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var neighbourPosition = position + new Vector2Int(dx,dy);
                    _cellsDictionary.TryGetValue(neighbourPosition, out ICell cell);
                    
                    if (cell != null && cell.CellVisitor is TVisitor)
                        yield return new Tuple<ICell, Vector2Int>(cell, neighbourPosition);
                }
            }
        }
        public IEnumerable<Tuple<ICell, Vector2Int>> GetCellNeighbours(Vector2Int position)
        {
            foreach (var positionOffset in _neighborOffsets)
            {
                var neighbourPosition = positionOffset + position;
                _cellsDictionary.TryGetValue(neighbourPosition, out ICell cell);

                if (cell != null)
                    yield return new Tuple<ICell, Vector2Int>(cell, neighbourPosition);
            }
        }

        public IEnumerable<ICell> GetCellNeighbours(ICell cell)
        {
            foreach (var positionOffset in _neighborOffsets)
            {
                var neighbourPosition = positionOffset + cell.Position;
                _cellsDictionary.TryGetValue(neighbourPosition, out ICell neighbourCell);

                if (neighbourCell != null)
                    yield return neighbourCell;
            }
        }
    }
}