using UnityEngine;

namespace Gameplay.Map.Building.ElectricityPoles.RangeView
{
    public class WirePoleRangeObject: MonoBehaviour
    {
        [SerializeField] private WirePoleMapBuilding wirePoleMapBuilding;
        
        [SerializeField] private float baseSingleCellScale;
        private void OnEnable()
        {
            var radius = wirePoleMapBuilding.WirePoleBuildingCore.Radius;
            var resultScale = radius * baseSingleCellScale;
            transform.localScale = new Vector3(resultScale, resultScale, resultScale);
        }
    }
}