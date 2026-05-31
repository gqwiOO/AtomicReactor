using Gameplay.Buildings.View;
using Gameplay.Map.Building.Electricity;
using Gameplay.MapUI.Views;
using UnityEngine;

namespace Gameplay.Map.Building.Generators
{
    public abstract class BaseGeneratorView: BaseMapObjectView
    {
        [field: SerializeField] public ElectricityCurrentValueView electricityCurrentValueView { get; private set; }
        public abstract override void Init(BuildingMapObject buildingMapObject);

        protected void InitElectricityView(IElectricityContainer electricityContainer)
        {
            electricityCurrentValueView.Init(electricityContainer);
        }
    }
}