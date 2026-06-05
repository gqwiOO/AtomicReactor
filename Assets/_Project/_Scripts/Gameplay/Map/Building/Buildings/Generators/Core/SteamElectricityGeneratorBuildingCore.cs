using Gameplay.Inventories;

namespace Gameplay.Map.Building.Generators.Core
{
    public class SteamElectricityGeneratorBuildingCore : BaseGeneratorBuildingCore
    {
        public SingleCellInventory _itemContainer;

        public SteamElectricityGeneratorBuildingCore(GeneratorBuildingSettingsData generatorBuildingSettingsData) : base(generatorBuildingSettingsData)
        {
            _itemContainer = new SingleCellInventory();
        }

        public override void Tick(float time)
        {
            if(true)
                base.Tick(time);
        }
    }
}