using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Building.Generators
{
    public class WindGeneratorMapObject : BaseElectricityGeneratorMapObject
    {
        private WindGeneratorBuildingCore _windGeneratorBuildingCore;
        private WindGeneratorBuildingSettingsDataAsset _buildingSettings;

        public override IElectricityProvider ElectricityProvider { get; protected set; }
        public override float Power => _windGeneratorBuildingCore.Power;
        public override async UniTask Init(Vector2Int cellPosition)
        {
            _buildingSettings = _buildingsSettingsProvider.GetBuildingSettings(Key) as WindGeneratorBuildingSettingsDataAsset;
            _windGeneratorBuildingCore = new WindGeneratorBuildingCore(_buildingSettings?.GeneratorBuildingSettingsData);
            
            ElectricityProvider = new ElectricityProvider();
            ElectricityProvider.SetContainer(_windGeneratorBuildingCore.ElectricityContainer);
            
            base.Init(cellPosition);
        }

        public override void Tick()
        {
            _windGeneratorBuildingCore.Tick(Time.deltaTime);
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            
        }
    }
}