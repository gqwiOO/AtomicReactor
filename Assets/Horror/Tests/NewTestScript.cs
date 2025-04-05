using System.Collections;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    
    [UnityTest]
    public IEnumerator ElectricityDividingTest()
    {
        IElectricityProvider electricityProvider = new ElectricityProvider();
        IElectricityContainer providerContainer = new ElectricityContainer(float.MaxValue,0);
        electricityProvider.SetContainer(providerContainer);
        
        IElectricityContainer container_1 = new ElectricityContainer(float.MaxValue,0);
        IElectricityContainer container_2 = new ElectricityContainer(float.MaxValue,0);
        
        electricityProvider.RegisterContainer(container_1);
        electricityProvider.RegisterContainer(container_2);
        
        providerContainer.Add(5000);

        Assert.AreEqual(2500, container_1.CurrentValue);
        yield return null;
    }
}
