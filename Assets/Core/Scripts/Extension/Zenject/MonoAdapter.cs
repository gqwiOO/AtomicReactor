using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Core.Extention.Zenject
{
    public class MonoAdapter: MonoInstaller
    {
        [SerializeField] private List<MonoInstaller> _installers;
        public override void InstallBindings()
        {
            foreach (var installer in _installers)
            {
                Container.Inject(installer);
                installer.InstallBindings();
            }
        }
    }
}