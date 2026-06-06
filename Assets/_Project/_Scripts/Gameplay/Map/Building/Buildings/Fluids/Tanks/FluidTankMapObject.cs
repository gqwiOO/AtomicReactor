using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Cell;
using Gameplay.MapUI.Views;
using Gameplay.Transportation.WaterPipeSystem;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Fluids.Tanks
{
    public class FluidTankMapObject: BuildingMapObject, IFluidInsertionTarget
    {
        private IBuildingsSettingsProvider _buildingsSettingsProvider;
        private IFluidTankDataSettings _buildingSettings;
        public IFluidContainer InsertionFluidContainer { get; private set; }

        [Inject]
        private void Construct(IBuildingsSettingsProvider buildingsSettingsProvider)
        {
            _buildingsSettingsProvider = buildingsSettingsProvider;
        }
        
        public override async UniTask Init(Vector2Int cellPosition)
        {
            _buildingSettings = base._buildingsSettingsProvider.GetBuildingSettings(Key) as IFluidTankDataSettings;
            InsertionFluidContainer = new FluidContainer(FluidType.None, _buildingSettings.MaxCapacity);
            await base.Init(cellPosition);
        }

        public override void Tick()
        {
            
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            
        }
    }

    public interface IFluidInsertionTarget
    {
        IFluidContainer InsertionFluidContainer { get; }

        void Add(float value)
        {
            InsertionFluidContainer.FloatContainer.Add(value);
        }

        void Remove(float value)
        {
            InsertionFluidContainer.FloatContainer.Remove(value);
        }
    }
}