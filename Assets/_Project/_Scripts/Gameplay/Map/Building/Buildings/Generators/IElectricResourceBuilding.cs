using Gameplay.Map.Building.Electricity.Consumer;

namespace Gameplay.Map.Building.Generators
{
    public interface IElectricResourceBuilding
    {
        public IElectricityProvider ElectricityProvider { get;}
        
        public float Power { get; }
    }
}