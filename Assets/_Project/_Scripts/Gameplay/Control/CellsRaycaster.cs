using System;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Control
{
    public class CellsRaycaster : MonoBehaviour, ICellMapListener
    {
        [SerializeField] private Camera mainCamera;

        private CellComponent _previousSelectedCell;

        public event Action<CellComponent> OnCellPointed;
        
        public CellComponent CurrentCell { get; private set; }
        void Update()
        {
            RaycastToMouse();
        }

        private void RaycastToMouse()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CellComponent cell = hit.collider.GetComponentInParent<CellComponent>();
                if (cell != null && cell != _previousSelectedCell)
                {
                    CurrentCell = cell;
                    OnCellPointed?.Invoke(cell);
                    if (cell.CellVisitor == null)
                        cell.CellSelectHandler.SelectCell();
                    if (_previousSelectedCell != null)
                        _previousSelectedCell.CellSelectHandler.UnselectCell();
                    _previousSelectedCell = cell;
                }
            }
        }
    }

    public interface ICellMapListener
    {
        event Action<CellComponent> OnCellPointed;
        
        public CellComponent CurrentCell { get; }
    }
}