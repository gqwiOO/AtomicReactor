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
        }
    }
}