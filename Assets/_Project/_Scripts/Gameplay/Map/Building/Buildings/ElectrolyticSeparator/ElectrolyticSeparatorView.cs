using Gameplay.Buildings.View;
using Gameplay.MapUI.Views;
using Gameplay.UI.Views;
using UnityEngine;

namespace Gameplay.Map.Building.ElectrolyticSeparator
{
    public class ElectrolyticSeparatorView: BaseMapObjectView
    {
        [SerializeField] private ElectricityCurrentValueView electricityView;
        [SerializeField] private FluidContainerView inputView;
        [SerializeField] private FluidContainerView outputView_1;
        [SerializeField] private FluidContainerView outputView_2;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as ElectrolyticSeparatorBuilding);
        }

        private void SpecificInit(ElectrolyticSeparatorBuilding buildingMapObject)
        {
            electricityView.Init(buildingMapObject.ElectricityContainer);
            inputView.Init(buildingMapObject.InsertionFluidContainer);
            outputView_1.Init(buildingMapObject.ExtractionFluidContainer_1);
            outputView_2.Init(buildingMapObject.ExtractionFluidContainer_2);
        }
    }
}