using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Building;
using Gameplay.Map.Building.Destroy;
using Gameplay.Map.Building.Factory;
using Gameplay.Map.Building.Placer;
using Gameplay.Map.Building.Selector;
using Gameplay.Map.Building.Validator;
using Gameplay.Map.Chunk;
using Gameplay.Map.CellsService;
using Gameplay.Map.Generating.Ore;
using Gameplay.Map.Generating.Wood;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Installer
{
    public class MapInstaller: MonoInstaller
    {
        [SerializeField] private BuildingMapSpawnSelector buildingMapSpawnSelector;
        [SerializeField] private BuildingMapFactory buildingMapFactory;
        [SerializeField] private BuildingSelector buildingSelector;
        [SerializeField] private BuildingDestroyHandler buildingDestroyHandler;

        public override void InstallBindings()
        {
            Container.Bind<IBuildingMapSpawnSelector>().FromInstance(buildingMapSpawnSelector).AsSingle();
            Container.Bind<IBuildingMapFactory>().FromInstance(buildingMapFactory).AsSingle();
            Container.BindInterfacesAndSelfTo<MapCellsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapChunkService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingCellPlacementValidator>().AsSingle();
            Container.BindInterfacesAndSelfTo<BiomesService>().AsSingle();
            Container.Bind<IWoodGenerator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IOreGenerator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IBuildingSelector>().FromInstance(buildingSelector).AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingDestroyHandler>().FromInstance(buildingDestroyHandler).AsSingle();
        }
    }
}