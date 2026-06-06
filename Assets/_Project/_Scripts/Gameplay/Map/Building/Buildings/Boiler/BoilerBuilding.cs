using Cysharp.Threading.Tasks;
using Gameplay.Fuel;
using Gameplay.Map.Building.Fluids.Tanks;
using Gameplay.Map.Cell;
using Gameplay.Transportation.ItemPipeSystem;
using Gameplay.Transportation.WaterPipeSystem;
using UnityEngine;

namespace Gameplay.Map.Building.Boiler
{
    public class BoilerBuilding: BuildingMapObject, IItemInsertionTarget, IFluidExtractionSource, IFluidInsertionTarget
    {
        private BoilerCore _core;

        public IFloatContainer WaterContainer => _core.WaterContainer; 
        public IFluidProvider SteamContainer => _core.SteamContainer; 
        public FuelContainer FuelContainer => _core.FuelContainer; 
        public IFluidProvider ExtractionFluidProvider => _core.SteamContainer;
        public IFloatContainer InsertionFloatContainer => _core.WaterContainer;
        public override async UniTask Init(Vector2Int cellPosition)
        {
            BoilerBuildingSettingsDataAsset settings = _buildingsSettingsProvider.GetBuildingSettings(Key) as BoilerBuildingSettingsDataAsset;
            _core = new BoilerCore(settings.AcceptedFuels, settings.FuelCapacity, settings.WaterCapacity, settings.SteamCapacity,
                settings.RequiredHeatPerWaterUnit, settings.RequiredWaterPerSteamUnit, settings.BuildingSidesData);
            await base.Init(cellPosition);
        }

        public override void Tick()
        {
            _core.Tick(Time.deltaTime);
        }
        
        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            
        }

        public bool CanInsertFromPipe(int itemId, int amount = 1) 
            => _core != null && _core.FuelContainer.CanAddItemFuel(itemId, amount);

        public void InsertFromPipe(int itemId, int amount) 
            => _core.FuelContainer.ItemFuelContainer.Add(itemId, amount);

    }
}