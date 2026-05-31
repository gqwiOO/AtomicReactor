using System.Collections.Generic;
using Mechanics.Pools;
using UnityEngine;
using Zenject;

namespace Gameplay.Inventories.Views
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private PoolGameObjects _pool;
        [SerializeField] private Transform _container;

        private IInventory _inventory;
        private readonly Dictionary<InventoryCell, InventoryCellView> _cellViews = new();

        [Inject]
        private void Construct()
        {

        }

        public void Init(IInventory inventory)
        {
            _pool.Initialize();
            if (_inventory != inventory)
                ClearView();
            _inventory = inventory;
            RefreshView();
        }

        private void ClearView()
        {
            foreach (var inventoryCellView in _cellViews)
            {
                _pool.Push(inventoryCellView.Value.GetComponent<IPoolObject>());
            }
            _cellViews.Clear();
        }

        private void RefreshView()
        {
            foreach (var cell in _inventory.InventoryCells)
            {
                if (!_cellViews.ContainsKey(cell))
                {
                    var viewObject = _pool.Pull().GetOwner();
                    viewObject.gameObject.SetActive(true);
                    viewObject.transform.SetParent(_container);
                    var cellView = viewObject.GetComponent<InventoryCellView>();
                    cellView.Init(cell);
                    _cellViews[cell] = cellView;
                    cell.OnCellUpdated += () => cellView.UpdateView();
                }
            }
        }

        private void RemoveEmptyCells()
        {
            var toRemove = new List<InventoryCell>();
            foreach (var pair in _cellViews)
            {
                if (pair.Key.Amount <= 0)
                {
                    _pool.Push(pair.Value.GetComponent<IPoolObject>());
                    toRemove.Add(pair.Key);
                }
            }

            foreach (var cell in toRemove)
            {
                _cellViews.Remove(cell);
            }
        }
    }
}