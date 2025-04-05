using Gameplay.Map.Building;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem.Assets
{
    [CreateAssetMenu(menuName = "Core/Buildings/BasePipeDataAsset", fileName = "BasePipeDataAsset")]
    public class BasePipeDataAsset: BuildingSettingsDataAsset, IFluidPipeData
    {
        [field: SerializeField] public float PipeCapacity { get; private set; }
    }

    public interface IFluidPipeData
    {
        public float PipeCapacity { get; }
    }
}