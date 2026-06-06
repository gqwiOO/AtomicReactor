using Gameplay.MapUI.Views;
using Gameplay.UI.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Map.Building.Fluids.View
{
    public class WaterPumpMapObjectView: BaseMapObjectView
    {
        [SerializeField] private FloatContainerView floatContainerView;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as WaterPumpMapObject);
        }

        private void SpecificInit(WaterPumpMapObject waterPumpMapObject)
        {
            floatContainerView.Init(waterPumpMapObject.ExtractionFluidContainer.FloatContainer);
        }
    }
}