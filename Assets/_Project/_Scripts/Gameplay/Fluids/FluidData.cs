using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    [CreateAssetMenu(menuName = "Core/Resources/Fluids/FluidData", fileName = "FluidData")]
    public class FluidData: ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}