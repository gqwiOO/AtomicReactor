using Gameplay.Inventories.Views;
using Gameplay.MapUI.Views;
using TMPro;
using UnityEngine;

namespace Gameplay.Map.Building
{
    public class ItemMinerView : BaseMapObjectView
    {
        [SerializeField] private InventoryCellView inventoryCellView;
        [SerializeField] private TMP_Text miningRateText;

        private BaseItemMinerMapObject _miner;

        public override void Init(BuildingMapObject buildingMapObject)
        {
            _miner = buildingMapObject as BaseItemMinerMapObject;
            if (_miner == null) return;

            inventoryCellView.Init(_miner.InventoryCell);
            _miner.InventoryCell.OnCellUpdated += UpdateRateText;
            UpdateRateText();
        }

        private void UpdateRateText()
        {
            if (miningRateText == null) return;
            float rate = _miner.MiningRatePerSecond;
            miningRateText.text = rate > 0f ? $"{rate:F2}/s" : "—";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_miner?.InventoryCell != null)
                _miner.InventoryCell.OnCellUpdated -= UpdateRateText;
        }
    }
}
