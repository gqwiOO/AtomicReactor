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

        private SingleCellInventory _singleCellInventory = new();

        private float _lastReturnTime = -1f;
        private float _lastCycleDuration;

        protected override IInventory Storage => _singleCellInventory;
        public override int MinedItemId => miningResourceItem != null ? miningResourceItem.ItemId : -1;
        public override float MiningRatePerSecond =>
            _lastCycleDuration > 0f ? 5f / _lastCycleDuration : 0f;

        public override async UniTask Init(Vector2Int cellPosition)
        {
            base.Init(cellPosition);
            var unit = CreateUnit();
            unit.SetBarrack(this);
            unit.StartWorking();
        }

        public override void Tick() { }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction) { }

        protected override void Unit_OnReturnedToBarrack()
        {
            float now = Time.time;
            if (_lastReturnTime >= 0f)
                _lastCycleDuration = now - _lastReturnTime;
            _lastReturnTime = now;

            _singleCellInventory.Add(miningResourceItem.ItemId, 5);
        }
    }
}
