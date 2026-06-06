using System;
using Gameplay.Map.Cell;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Control
{
    public class CellsRaycaster : MonoBehaviour, ICellMapListener
    {
        private const string MAP_LAYER = "Map";
        
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
            // bool throughUI = Physics.Raycast(ray, out RaycastHit uiHit, 1000, 1 << LayerMask.NameToLayer("UI"));
            // if(throughUI) return;
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, 1 << LayerMask.NameToLayer(MAP_LAYER) ))
            {
                CellComponent cell = hit.collider.GetComponentInParent<CellComponent>();
                if (cell != null)
                {
                    Debug.Log($"[CellsRaycaster] Selected Cell - \n" +
                              $"Position = {cell.Position.x}, {cell.Position.y} \n" + 
                              $"WorldPosition = {cell.WorldPosition.x}, {cell.WorldPosition.z} \n" + 
                              $"Type = {cell.CellType}");
                    if (cell != _previousSelectedCell)
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
    }

    public interface ICellMapListener
    {
        event Action<CellComponent> OnCellPointed;
        
        public CellComponent CurrentCell { get; }
    }
}