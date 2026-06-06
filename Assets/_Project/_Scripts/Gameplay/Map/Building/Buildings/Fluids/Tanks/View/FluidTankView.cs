using Gameplay.MapUI.Views;
using Gameplay.Transportation.WaterPipeSystem;
using Gameplay.UI.Views;
using UnityEngine;

namespace Gameplay.Map.Building.Fluids.Tanks.View
{
    public class FluidTankView: BaseMapObjectView
    {
        [SerializeField] private FloatContainerView floatContainerView;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as IFluidInsertionTarget);
        }

        private void SpecificInit(IFluidInsertionTarget fluidBuildingContainer)
        {
            floatContainerView.Init(fluidBuildingContainer.InsertionFloatContainer);
        }
    }
}