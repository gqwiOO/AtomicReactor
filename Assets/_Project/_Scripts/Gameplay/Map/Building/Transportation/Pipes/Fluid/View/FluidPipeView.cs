using Gameplay.Map.Building;
using Gameplay.Map.Building.Fluids;
using Gameplay.MapUI.Views;
using Gameplay.UI.Views;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem.View
{
    public class FluidPipeView: BaseMapObjectView
    {
        [SerializeField] private FloatContainerView floatContainerView;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as IPipe);
        }

        private void SpecificInit(IPipe pipe)
        {
            floatContainerView.Init(pipe.ParentPipeSystem.FloatContainer);
        }
    }
}