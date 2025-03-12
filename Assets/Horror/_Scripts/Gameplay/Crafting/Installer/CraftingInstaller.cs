using UnityEngine;
using Zenject;

namespace Gameplay.Crafting.Installer
{
    public class CraftingInstaller: MonoInstaller
    {

        [SerializeField] private CraftService craftService;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CraftService>().FromInstance(craftService).AsSingle();
        }
    }
}