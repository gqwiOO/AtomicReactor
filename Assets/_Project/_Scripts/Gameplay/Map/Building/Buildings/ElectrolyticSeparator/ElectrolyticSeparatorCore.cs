using System.Collections.Generic;
using Gameplay.Map.Building.CraftBuilding;
using Gameplay.Transportation.WaterPipeSystem;

namespace Gameplay.Map.Building.ElectrolyticSeparator
{
    public class ElectrolyticSeparatorCore : BaseElectricityRequiredBuildingCore
    {
        public IFluidContainer InputFluidContainer { get; private set; }
        public IFluidContainer Output1FluidContainer { get; private set; }
        public IFluidContainer Output2FluidContainer { get; private set; } 
        private ElectrolyticSeparatorSettingsData _electrolyticSeparatorSettingsData;

        private List<ElectrolyticSeparation> _possibleSeparations;

        public ElectrolyticSeparatorCore(ElectrolyticSeparatorSettingsData electrolyticSeparatorSettingsData)
        {
            _electrolyticSeparatorSettingsData = electrolyticSeparatorSettingsData;
            _possibleSeparations = electrolyticSeparatorSettingsData.Separations;
            
            InputFluidContainer = new FluidContainer(FluidType.None, _electrolyticSeparatorSettingsData.M3_InputContainerCapacity);
            Output1FluidContainer = new FluidContainer(FluidType.None, _electrolyticSeparatorSettingsData.M3_Output1ContainerCapacity);
            Output2FluidContainer = new FluidContainer(FluidType.None, _electrolyticSeparatorSettingsData.M3_Output2ContainerCapacity);
        }
        
        public override void Tick(float time)
        {
            if (InputFluidContainer.IsEmpty())
                return;
            
            ElectrolyticSeparation separation = _possibleSeparations.Find(s => s.FluidType == InputFluidContainer.FluidType);
            if (separation == null)
                return;
            
            // output1 filled with other fluid
            if(!Output1FluidContainer.IsEmpty() && Output1FluidContainer.FluidType != separation.OutputFluidType[0].FluidType)
                return;

            // output2 filled with other fluid
            if (!Output2FluidContainer.IsEmpty() && separation.OutputFluidType.Count >= 2 &&
                Output2FluidContainer.FluidType != separation.OutputFluidType[1].FluidType)
                return;

            float requiredElectricity = separation.KW_PowerPerM3Input * time;
            if (!ElectricityContainer.CanConsume(requiredElectricity))
                return;
            
            ElectricityContainer.Consume(requiredElectricity);
            
            
            InputFluidContainer.ExtractFluid(separation.H_SpeedPerM3Input * time);
            Output1FluidContainer.AddFluid(separation.OutputFluidType[0].FluidType, separation.OutputFluidType[0].M3_Amount * time * separation.H_SpeedPerM3Input);

            if (separation.OutputFluidType.Count >= 2)
            {
                Output2FluidContainer.AddFluid(separation.OutputFluidType[1].FluidType, separation.OutputFluidType[1].M3_Amount * time * separation.H_SpeedPerM3Input);
            }
        }
    }
}