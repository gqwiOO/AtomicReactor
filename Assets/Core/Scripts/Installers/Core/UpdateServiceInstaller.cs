using _Project.Core.Services.UpdateService;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class UpdateServiceInstaller: MonoInstaller
    {
        [SerializeField] private UpdateService updateService;
        
        public override void InstallBindings() 
            => Container.Bind<IUpdateService>().To<UpdateService>().FromInstance(updateService);
    }
}