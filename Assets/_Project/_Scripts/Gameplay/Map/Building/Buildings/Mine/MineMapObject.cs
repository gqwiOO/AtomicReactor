using Cysharp.Threading.Tasks;
using Gameplay.Inventories;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Mine
{
    public class MineMapObject : BaseItemMinerMapObject, IElectricBuildingCore
    {
        private MineBuildingCore _core;
        private IMapCellsService _mapCellsService;

        public IElectricityContainer ElectricityContainer => _core.ElectricityContainer;

        protected override IInventory Storage => _core?.Storage;
        public override int MinedItemId => _core?.OreItemId ?? -1;
        public override float MiningRatePerSecond => _core?.MiningRatePerSecond ?? 0f;

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }

        public override async UniTask Init(Vector2Int cellPosition)
        {
            var settings = _buildingsSettingsProvider.GetBuildingSettings(Key) as MineBuildingSettingsDataAsset;
            _core = new MineBuildingCore(settings.MineSettings);

            ICell cell = _mapCellsService.GetCell(cellPosition);
            _core.SetOreItemId(settings.MineSettings.GetItemIdForCellType(cell.CellType));

            base.Init(cellPosition);
        }

        public override void Tick()
        {
            _core.Tick(Time.deltaTime);
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            _core.OnNeighbourUpdated(neighborCell, direction);
        }
    }
}
