using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Map.Building.ElectrolyticSeparator
{
    [CreateAssetMenu(fileName = "ElectrolyticSeparatorSettingsData", menuName = "Core/Buildings/ElectrolyticSeparatorSettingsData")]
    public class ElectrolyticSeparatorSettingsData : BuildingSettingsDataAsset
    {
        [field: SerializeField] public List<ElectrolyticSeparation> Separations { get; private set; }
        [field: SerializeField] public float M3_InputContainerCapacity { get; private set; }
        [field: SerializeField] public float M3_Output1ContainerCapacity { get; private set; }
        [field: SerializeField] public float M3_Output2ContainerCapacity { get; private set; }
    }
}