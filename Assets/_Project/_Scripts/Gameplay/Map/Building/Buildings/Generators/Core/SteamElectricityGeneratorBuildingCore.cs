using Gameplay.Map.Building.Generators.HeatGenerator;
using Gameplay.Transportation.WaterPipeSystem;

namespace Gameplay.Map.Building.Generators.Core
{
    public class SteamElectricityGeneratorBuildingCore : BaseGeneratorBuildingCore
    {
        public FluidContainer SteamContainer { get; private set; }

        public SteamElectricityGeneratorBuildingCore(GeneratorBuildingSettingsData generatorBuildingSettingsData) : base(generatorBuildingSettingsData)
        {
            SteamGeneratorSettingsData steamGeneratorSettingsData =
                generatorBuildingSettingsData as SteamGeneratorSettingsData;
            SteamContainer = new FluidContainer(FluidType.Steam, steamGeneratorSettingsData.SteamContainerCapacity);
        }

        public override void Tick(float time)
        {
            base.Tick(time);
        }
    }
}