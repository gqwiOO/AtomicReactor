namespace Gameplay.Map.Building.Generators.HeatGenerator
{
    public class HeatGeneratorView : BaseGeneratorView
    {
        public override void Init(BuildingMapObject buildingMapObject)
            => SpecificInit(buildingMapObject as HeatGeneratorMapObject);

        private void SpecificInit(HeatGeneratorMapObject heatGenerator)
            => InitElectricityView(heatGenerator.ElectricityProvider.ElectricityContainer);
    }
}
