using Core.Timer;
using Core.Timer.Service;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class TimerInstaller: MonoInstaller
    {
        [SerializeField] private TimersProvider timersProvider;
        public override void InstallBindings()
        {
            Container.Bind<ITimerService>().To<TimerService>().AsSingle().WithArguments(timersProvider);
        }
    }
}