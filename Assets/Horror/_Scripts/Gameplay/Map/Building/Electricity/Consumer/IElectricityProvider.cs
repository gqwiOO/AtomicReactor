namespace Gameplay.Map.Building.Electricity.Consumer
{
    public interface IElectricityProvider
    {
        IElectricityContainer ElectricityContainer { get; }
        
        void SetContainer(IElectricityContainer electricityContainer);
        
        void RegisterContainer(IElectricityContainer electricityContainer);
        void UnregisterContainer(IElectricityContainer electricityContainer);
        void ExtractAllEnergy();

        // void ConsumeElectricity(float value);
        //
        // bool CanConsume(float value);
    }
}