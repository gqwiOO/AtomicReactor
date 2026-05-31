namespace Gameplay.Map.Building.Generators.Core
{
    public class SteamElectricityGeneratorBuildingCore : BaseGeneratorBuildingCore
    {
        public IItemContainer _itemContainer;
        
        public SteamElectricityGeneratorBuildingCore(GeneratorBuildingSettingsData generatorBuildingSettingsData) : base(generatorBuildingSettingsData)
        {
            _itemContainer = new ItemContainer();
        }

        public override void Tick(float time)
        {
            if(true)
                base.Tick(time);
        }
    }
}