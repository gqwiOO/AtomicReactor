using Gameplay.Map.Building.Items.Provider;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Items.Installer
{
    public class ItemsInstaller: MonoInstaller
    {
        [SerializeField]
        private ItemsDataProvider itemsDataProvider;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ItemsDataProvider>().FromInstance(itemsDataProvider).AsSingle();
        }
    }
}