using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.CellsService
{
    public interface IMapCellsService
    {
        Vector2Int MapSize { get; }
        
        IEnumerable<Tuple<ICell, Vector2Int>> GetCellNeighbours(Vector2Int position, int radius);
        IEnumerable<Tuple<ICell, Vector2Int>> GetCellNeighboursWithVisitor<TVisitor>(Vector2Int position, int radius) where TVisitor: ICellVisitor;
        IEnumerable<Tuple<ICell, Vector2Int>> GetCellNeighboursWithPositions(Vector2Int position);
        Dictionary<Vector2Int,ICell> GetCellNeighboursWithPositionsDictionary(Vector2Int position);
        Dictionary<ICell,CellType> GetCellsWithTypes(Vector2Int position);
        IEnumerable<ICell> GetCellNeighbours(ICell cell);
        IEnumerable<ICell> GetCellNeighbours(Vector2Int position);
        ICell GetCell(Vector2Int cell);
        void InitCells(Dictionary<Vector2Int, ICell> cells, Vector2Int mapSize);

        ICell GetClosestCellWithVisitor<TVisitor>(out TVisitor visitor, Vector2Int targetCell) where TVisitor : class,ICellVisitor ;
        ICell GetClosestEmptyCellWithNeighbourCellVisitor<TVisitor>(out TVisitor visitor, out ICell visitorCell, Vector2Int targetCell) where TVisitor : class,ICellVisitor ;
        Task<ICell> GetCellWithVisitorAsync<TVisitor>() where TVisitor : class,ICellVisitor;
    }
}