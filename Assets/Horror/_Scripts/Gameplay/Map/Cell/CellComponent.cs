using System;
using Gameplay.Map.CellsService;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Cell
{
    public class CellComponent : MonoBehaviour, ICell
    {
        private IMapCellsService _mapCellsService;

        [field: SerializeField]
        public CellSelectHandler CellSelectHandler { get; private set; }
        
        public Vector2Int Position { get; private set; }
        public Vector3 WorldPosition => transform.position;
        public ICellVisitor CellVisitor { get; private set; }
        
        public event Action<ICell> OnUpdated;
        
        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }
        public void NotifyAboutNeighbourUpdated(ICell cell)
        {
            Vector2Int direction = cell.Position - Position;
            CellVisitor?.NotifyAboutNeighborUpdated(cell,direction);
        }

        public void RefreshMaterial()
        {
            CellSelectHandler.RefreshMaterial();
        }

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void SetVisitor(ICellVisitor cellVisitor)
        {
            if (CellVisitor != null)
                CellVisitor.OnUpdated -= CellVisitor_OnUpdated; 
            
            CellVisitor = cellVisitor;
            OnUpdated?.Invoke(this);
            if(cellVisitor != null)
                cellVisitor.OnUpdated += CellVisitor_OnUpdated;
        }

        private void CellVisitor_OnUpdated()
        {
            foreach (ICell cellNeighbour in _mapCellsService.GetCellNeighbours(this))
                CellVisitor?.NotifyAboutNeighborUpdated(cellNeighbour,cellNeighbour.Position - Position);
            OnUpdated?.Invoke(this);
        }
    }
}