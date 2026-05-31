using Gameplay.Map.Building.Electricity.Consumer;

namespace Gameplay.Map.Building.Generators
{
    public abstract class BaseElectricityGeneratorMapObject: BuildingMapObject, IElectricResourceBuilding
    {
        public abstract IElectricityProvider ElectricityProvider { get; protected set; }
        public abstract float Power { get;}
    }
}