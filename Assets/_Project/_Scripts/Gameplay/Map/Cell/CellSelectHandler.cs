using System;
using UnityEngine;

namespace Gameplay.Map.Cell
{
    public abstract class CellSelectHandler: MonoBehaviour
    {
        [SerializeField] 
        protected Renderer _renderer;
        
        public abstract void SelectCell();
        public abstract void UnselectCell();

        public void RefreshMaterial() => _renderer.material = Instantiate(_renderer.material);
    }
}