using System;
using Gameplay.Inventories;
using Gameplay.Map.Building.CraftBuilding;

namespace Gameplay.Map.Building.Mine
{
    [Serializable]
    public class MineBuildingCore : BaseElectricityRequiredBuildingCore
    {
        private readonly MineBuildingSettingsData _settings;
        private int _oreItemId = -1;
        private float _miningTimer;

        public IInventory Storage { get; }
        public int OreItemId => _oreItemId;
        public float MiningRatePerSecond => _settings is { MiningInterval: > 0f }
            ? 1f / _settings.MiningInterval
            : 0f;

        public MineBuildingCore(MineBuildingSettingsData settings) : base()
        {
            _settings = settings;
            Storage = new Inventory(1, settings.StorageCellCapacity);
        }

        public void SetOreItemId(int itemId) => _oreItemId = itemId;

        public override void Tick(float time)
        {
            if (_oreItemId == -1) return;
            if (!ElectricityContainer.CanConsume(_settings.ElectricityPerExtraction)) return;
            if (!Storage.CanAdd(_oreItemId, 1)) return;

            _miningTimer += time;
            if (_miningTimer < _settings.MiningInterval) return;
            _miningTimer = 0;

            ElectricityContainer.Consume(_settings.ElectricityPerExtraction);
            Storage.Add(_oreItemId, 1);
        }
    }
}
