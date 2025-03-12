using Gameplay.Map.Building.SettingsProvider;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Installer
{
    public class BuildingsMapSettingsProviderInstaller: MonoInstaller
    {
        [SerializeField] 
        private BuildingsSettingsProvider buildingsSettingsProvider;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BuildingsSettingsProvider>().FromInstance(buildingsSettingsProvider)
                .AsSingle();
        }
    }
}