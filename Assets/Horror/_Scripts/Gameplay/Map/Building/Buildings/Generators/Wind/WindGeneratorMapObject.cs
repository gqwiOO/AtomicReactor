using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Generators
{
    public class WindGeneratorMapObject : BaseElectricityGeneratorMapObject
    {
        private WindGeneratorBuildingCore _windGeneratorBuildingCore;
        public override void Init(Vector2Int cellPosition)
        {
            var buildingSettings = _buildingsSettingsProvider.GetBuildingSettings(Key) as GeneratorBuildingSettingsDataAsset;
            _windGeneratorBuildingCore = new WindGeneratorBuildingCore(buildingSettings?.GeneratorBuildingSettingsData);
            
            ElectricityProvider = new ElectricityProvider();
            ElectricityProvider.SetContainer(_windGeneratorBuildingCore.ElectricityContainer);
            
            base.Init(cellPosition);
            
            base.Init(cellPosition);
        }

        public override void Tick()
        {
            
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            
        }

        public override IElectricityProvider ElectricityProvider { get; protected set; }
    }
}