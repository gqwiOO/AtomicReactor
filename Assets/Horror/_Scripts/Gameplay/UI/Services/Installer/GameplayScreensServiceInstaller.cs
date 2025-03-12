using UnityEngine;
using Zenject;

namespace Gameplay.UI.Services.Installer
{
    public class GameplayScreensServiceInstaller: MonoInstaller
    {
        [SerializeField] private GameplayScreensService gameplayScreensService;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameplayScreensService>().FromInstance(gameplayScreensService).AsSingle();
        }
    }
}