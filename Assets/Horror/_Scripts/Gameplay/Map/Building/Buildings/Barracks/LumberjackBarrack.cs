using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Inventories;
using Gameplay.Map.Building.Items.Data;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Units
{
    public class LumberjackBarrack : BaseBarrack
    {
        [SerializeField] 
        private ItemDataAsset miningResourceItem;
        
        private SingleCellInventory _singleCellInventory = new ();

        private HashSet<IBarackUnit> _units = new();

        public InventoryCell InventoryCell => _singleCellInventory.InventoryCells.First();
        
        public override async UniTask Init(Vector2Int cellPosition)
        {
            base.Init(cellPosition);
            var unit = CreateUnit();
            unit.SetBarrack(this);
            unit.StartWorking();
        }

        public override void Tick()
        {
            
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
        }

        protected override void Unit_OnReturnedToBarrack()
        {
            _singleCellInventory.Add(miningResourceItem.ItemId,5);
        }
    }
}