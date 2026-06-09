using Gameplay.Control;
using Gameplay.Map.Control;
using Zenject;

namespace Gameplay
{
    public class GameplayInstaller: MonoInstaller   
    {
        public override void InstallBindings()
        {
            MapControl();
        }

        private void MapControl()
        {
            Container.BindInterfacesAndSelfTo<MapControlService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapBuildingControlTypeHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapDestroyingControlTypeHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapDefaultControlTypeHandler>().AsSingle();
        }
    }
}