using Cysharp.Threading.Tasks;
using Gameplay.Inventories;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Building.Fluids.Tanks;
using Gameplay.Transportation.ItemPipeSystem;
using Gameplay.Transportation.WaterPipeSystem;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Generators.HeatGenerator
{
    public class HeatGeneratorMapObject : BaseElectricityGeneratorMapObject, IFluidBuildingContainer, IItemInsertionTarget
    {
        private HeatGeneratorBuildingCore _core;

        public override IElectricityProvider ElectricityProvider { get; protected set; }
        public override float Power => _core.Power;

        public SingleCellInventory ItemFuelContainer => _core.ItemFuelContainer;

        // IFluidBuildingContainer — exposes the internal fluid fuel tank to the pipe system
        public IFloatContainer FloatContainer => _core.FluidFuelProvider.FloatContainer;
        public void Add(float value) => _core.FluidFuelProvider.AddFluid(_activeFuelFluidType, value);
        public void Remove(float value) => _core.FluidFuelProvider.ExtractFluid(value);

        private FluidType _activeFuelFluidType;

        public override async UniTask Init(Vector2Int cellPosition)
        {
            var asset = _buildingsSettingsProvider.GetBuildingSettings(Key) as HeatGeneratorBuildingSettingsDataAsset;
            _core = new HeatGeneratorBuildingCore(asset.HeatGeneratorSettingsData);

            ElectricityProvider = new ElectricityProvider();
            ElectricityProvider.SetContainer(_core.ElectricityContainer);

            await base.Init(cellPosition);
        }

        public override void Tick() => _core.Tick(Time.deltaTime);

        public bool CanInsertFromPipe(int itemId, int amount = 1) =>
            _core != null && _core.CanAddItemFuel(itemId, amount);

        public void InsertFromPipe(int itemId, int amount) =>
            _core.ItemFuelContainer.Add(itemId, amount);

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            if (neighborCell.CellVisitor is IPipe pipe && pipe.FluidType != FluidType.None)
                _activeFuelFluidType = pipe.FluidType;
        }
    }
}
