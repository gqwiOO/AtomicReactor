using UnityEngine;
using Zenject;

namespace Gameplay.MapUI
{
    public class MapUIInstaller: MonoInstaller
    {
        [SerializeField]
        private MapUIProvider mapUIProvider;
        
        [SerializeField]
        private MapUIHandler mapUIHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<IMapUIProvider>().FromInstance(mapUIProvider).AsSingle();
            Container.Bind<IMapUIHandler>().FromInstance(mapUIHandler).AsSingle();
        }
    }
}