using System;
using UnityEngine;

namespace Gameplay.Map.Cell
{
    [Serializable]
    public class CellColorChangeSelectHandler : CellSelectHandler
    {
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        
        public override void SelectCell()
        {
            _renderer.material.color = _selectedColor;
        }

        public override void UnselectCell()
        {
            _renderer.material.color = _unselectedColor;
        }
    }
}