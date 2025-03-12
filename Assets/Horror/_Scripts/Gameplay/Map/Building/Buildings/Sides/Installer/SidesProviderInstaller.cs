using Gameplay.Map.Building.Provider;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Installer
{
    public class SidesProviderInstaller: MonoInstaller
    {

        [SerializeField] private SidesSettingsProvider sidesSettingsProvider;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SidesSettingsProvider>().FromInstance(sidesSettingsProvider).AsSingle();
        }
    }
}