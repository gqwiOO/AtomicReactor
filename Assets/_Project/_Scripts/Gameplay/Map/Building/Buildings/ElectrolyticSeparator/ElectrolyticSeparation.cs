using System.Collections.Generic;
using Gameplay.Transportation.WaterPipeSystem;

namespace Gameplay.Map.Building.ElectrolyticSeparator
{
    [System.Serializable]
    public class ElectrolyticSeparation
    {
        public FluidType FluidType;
        public List<ElectrolyticSeparationResult> OutputFluidType;
        public float KW_PowerPerM3Input;
        public float H_SpeedPerM3Input;
    }
}