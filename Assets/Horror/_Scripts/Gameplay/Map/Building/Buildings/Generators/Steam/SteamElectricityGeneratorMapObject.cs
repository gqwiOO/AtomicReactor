using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Building.Generators.Core;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Generators.Steam
{
    public class SteamElectricityGeneratorMapObject : BaseElectricityGeneratorMapObject
    {
        private SteamElectricityGeneratorBuildingCore _steamElectricityGeneratorBuildingCore;

        public override IElectricityProvider ElectricityProvider { get; protected set; }

        public override void Init(Vector2Int cellPosition)
        {
            var buildingSettings = _buildingsSettingsProvider.GetBuildingSettings(Key) as GeneratorBuildingSettingsDataAsset;
            _steamElectricityGeneratorBuildingCore = new SteamElectricityGeneratorBuildingCore(buildingSettings?.GeneratorBuildingSettingsData);
            
            ElectricityProvider = new ElectricityProvider();
            ElectricityProvider.SetContainer(_steamElectricityGeneratorBuildingCore.ElectricityContainer);
            
            base.Init(cellPosition);
        }

        public override void Tick()
        {
            _steamElectricityGeneratorBuildingCore.Tick(Time.deltaTime);
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            
        }
    }
}