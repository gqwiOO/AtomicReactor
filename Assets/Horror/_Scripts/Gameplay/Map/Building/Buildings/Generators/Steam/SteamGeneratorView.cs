namespace Gameplay.Map.Building.Generators.Steam
{
    public class SteamGeneratorView : BaseGeneratorView
    {
        public override void Init(BuildingMapObject buildingMapObject)
            => SpecificInit(buildingMapObject as SteamElectricityGeneratorMapObject);
        private void SpecificInit(SteamElectricityGeneratorMapObject steamElectricityGeneratorMapObject) 
            => InitElectricityView(steamElectricityGeneratorMapObject.ElectricityProvider.ElectricityContainer);
    }
}