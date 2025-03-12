using Gameplay.Map.Building;
using Gameplay.Map.Building.Factory;
using Gameplay.Map.Building.Placer;
using Gameplay.Map.CellsService;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Installer
{
    public class MapInstaller: MonoInstaller
    {
        [SerializeField] private BuildingMapSpawnSelector buildingMapSpawnSelector;
        [SerializeField] private BuildingMapCellSelectorHandler buildingMapCellSelectorHandler;
        [SerializeField] private BuildingMapFactory buildingMapFactory;
        
        public override void InstallBindings()
        {
            Container.Bind<IBuildingMapSpawnSelector>().FromInstance(buildingMapSpawnSelector).AsSingle();
            Container.Bind<IBuildingMapCellSelectorHandler>().FromInstance(buildingMapCellSelectorHandler).AsSingle();
            Container.Bind<IBuildingMapFactory>().FromInstance(buildingMapFactory).AsSingle();
            Container.BindInterfacesAndSelfTo<MapCellsService>().AsSingle();
        }
    }
}