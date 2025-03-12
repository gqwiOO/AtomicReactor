using UnityEngine;
using Zenject;

namespace Mechanics.Raycast.Installer
{
    public class RaycastServiceInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RaycastService>().AsSingle();
        }
    }
}