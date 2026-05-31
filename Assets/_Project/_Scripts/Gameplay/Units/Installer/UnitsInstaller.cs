using Gameplay.Units.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.Installer
{
    public class UnitsInstaller: MonoInstaller
    {
        [SerializeField] private UnitsFactory factory;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UnitsFactory>().FromInstance(factory).AsSingle();
        }
    }
}