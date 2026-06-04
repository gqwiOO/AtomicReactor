using Gameplay.Map.Building.Items.Data;
using Gameplay.Transportation.WaterPipeSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Fuel
{
    [CreateAssetMenu(menuName = "Core/Fuel/FuelDataAsset", fileName = "FuelDataAsset")]
    public class FuelDataAsset : ScriptableObject
    {
        [field: SerializeField] public string FuelName { get; private set; }
        [field: SerializeField] public FuelSourceType SourceType { get; private set; }

        [Tooltip("Energy per unit: per 1 item, or per 1 litre of fluid/gas")]
        [field: SerializeField] public float EnergyInJoules { get; private set; }

        [field: SerializeField, ShowIf(nameof(IsItemFuel))]
        public ItemDataAsset ItemAsset { get; private set; }

        [field: SerializeField, ShowIf(nameof(IsFluidOrGas))]
        public FluidType FluidType { get; private set; }

        private bool IsItemFuel => SourceType == FuelSourceType.Item;
        private bool IsFluidOrGas => SourceType == FuelSourceType.Fluid || SourceType == FuelSourceType.Gas;
    }
}
