using System.ComponentModel;
using Gameplay.Control.Keyboard;
using UnityEngine;
using Zenject;

namespace Gameplay.Control
{
    public class MapControlInstaller: MonoInstaller
    {
        [SerializeField] 
        private CellsRaycaster _cellsRaycaster;
        [SerializeField] 
        private ControlInitializer controlInitializer;
        
        public override void InstallBindings()
        {
            Container.Bind<ICellMapListener>().FromInstance(_cellsRaycaster).AsSingle();
            Container.BindInterfacesAndSelfTo<ControlInitializer>().FromInstance(controlInitializer).AsSingle();
            Container.BindInterfacesAndSelfTo<KeyboardControl>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapKeyboardControl>().AsSingle();
        }
    }
}   