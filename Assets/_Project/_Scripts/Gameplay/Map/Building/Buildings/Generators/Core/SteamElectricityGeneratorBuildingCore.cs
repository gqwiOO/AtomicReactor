using System;
using Gameplay.Map.Building.Generators.HeatGenerator;
using Gameplay.Transportation.WaterPipeSystem;

namespace Gameplay.Map.Building.Generators.Core
{
    public class SteamElectricityGeneratorBuildingCore : BaseGeneratorBuildingCore
    {
        private SteamGeneratorSettingsData _steamGeneratorSettingsData;
        public FluidContainer SteamContainer { get; private set; }

        public SteamElectricityGeneratorBuildingCore(GeneratorBuildingSettingsData generatorBuildingSettingsData) : base(generatorBuildingSettingsData)
        {
            _steamGeneratorSettingsData = generatorBuildingSettingsData as SteamGeneratorSettingsData;
            SteamContainer = new FluidContainer(FluidType.Steam, _steamGeneratorSettingsData.M3_SteamContainerCapacity);
        }

        public override void Tick(float time)
        {
            if (SteamContainer.FloatContainer.CurrentValue < _steamGeneratorSettingsData.M3_SteamUsagePer_KW)
                return;
            
            ProduceEnergy();
        }
        
        // Example:
        // M3_SteamUsagePer_KWh - 1
        // KW_Power - 50
        //
        private void ProduceEnergy()
        {
            float maxPossibleKW = SteamContainer.FloatContainer.CurrentValue / _steamGeneratorSettingsData.M3_SteamUsagePer_KW;
            float wh = Math.Min(maxPossibleKW, _steamGeneratorSettingsData.KW_Power);
            SteamContainer.FloatContainer.Remove(_steamGeneratorSettingsData.M3_SteamUsagePer_KW * wh);
            ElectricityContainer.Add(ApplyEfficiency(wh));
        }
    }
}