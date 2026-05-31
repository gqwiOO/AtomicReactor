using System.Collections.Generic;
using Gameplay.Map.Building.Electricity.Consumer;

namespace Gameplay.Map.Building.Electricity
{
    public static class ElectricityTool
    {
        public static readonly float Voltage = 220f;


        public static void DivideElectricityForContainers(this IElectricityProvider electricityProvider, IEnumerable<IElectricityContainer> electricityContainers)
        {
            
        }
    }
}