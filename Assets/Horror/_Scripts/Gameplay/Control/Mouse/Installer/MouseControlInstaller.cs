using UnityEngine;
using Zenject;

namespace Gameplay.Control.Mouse.Installer
{
    public class MouseControlInstaller: MonoInstaller
    {
        [SerializeField]
        private MouseControlService mouseControlService;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MouseControlService>().FromInstance(mouseControlService).AsSingle();
        }
    }
}