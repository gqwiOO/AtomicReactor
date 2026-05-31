using UnityEngine;

namespace Gameplay.Map.Building.Fluids.Tanks
{
    [CreateAssetMenu(menuName = "Core/Buildings/FluidTankDataAsset", fileName = "FluidTankDataAsset")]

    public class FluidTankDataAsset: BuildingSettingsDataAsset, IFluidTankDataSettings
    {
        [field: SerializeField] public float MaxCapacity { get; private set; }
    }

    public interface IFluidTankDataSettings: IBuildingSettingsData
    {
        public float MaxCapacity { get; }
    }
}