using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Core.Services.UpdateService;
using Gameplay.Map.Building.Electricity;
using Gameplay.Map.Building.Electricity.Consumer;
using Gameplay.Map.Building.Generators;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using Gameplay.Map.Wires;
using UnityEngine;

namespace Gameplay.Map.Building.ElectricityPoles
{
    public class WirePoleBuildingCore: IBuildingCore
    {
        private readonly Vector2Int _cellPosition;
        private readonly IMapCellsService _mapCellsService;
        
        public WireSystem WireSystem;
        
        public BuildingSidesData BuildingSidesData { get; private set; }

        public int Radius = 5;
        private IUpdateService _updateService;
        private Transform _ropePoint;

        public WirePoleBuildingCore(Vector2Int cellPosition, IMapCellsService mapCellsService, IUpdateService updateService)
        {
            _updateService = updateService;
            _mapCellsService = mapCellsService;
            _cellPosition = cellPosition;
        }

        public void Init(Transform ropePoint)
        {
            _ropePoint = ropePoint;
            IEnumerable<BuildingMapObject> neighbourBuildings =  FindNeighbourBuildings();

            var buildingMapObjects = new HashSet<BuildingMapObject>(neighbourBuildings);
            InitWireSystem(buildingMapObjects);
            SortNeighbourBuildings(buildingMapObjects);
            
            SubscribeToNeighbourCells();
        }

        private void SubscribeToNeighbourCells()
        {
            //todo
            List<Tuple<ICell, Vector2Int>> neighbours = _mapCellsService.GetCellNeighbours(_cellPosition,Radius).ToList();

            foreach (var (cell, position) in neighbours)
            {
                cell.OnUpdated += RecheckNeighbourCellOnUpdate;
            }
        }

        private void RecheckNeighbourCellOnUpdate(ICell cell)
        {
            if (cell.CellVisitor is BuildingMapObject buildingMapObject && buildingMapObject.IsWorking)
            {
                if (buildingMapObject is IElectricResourceBuilding electricResourceBuilding && electricResourceBuilding.ElectricityProvider != null)
                    WireSystem.AddElectricityProvider(electricResourceBuilding.ElectricityProvider);
                if (buildingMapObject is IElectricBuildingCore electricBuilding && electricBuilding.ElectricityContainer != null)
                    WireSystem.AddElectricityContainer(electricBuilding.ElectricityContainer);
            }
        }

        private void SortNeighbourBuildings(IEnumerable<BuildingMapObject> neighbourBuildings)
        {
            List<IElectricityProvider> electricityProviders = new List<IElectricityProvider>();
            List<IElectricityContainer> electricBuildingsContainers = new List<IElectricityContainer>();
            
            foreach (var buildingMapObject in neighbourBuildings)
            {
                if(buildingMapObject is IElectricResourceBuilding electricResourceBuilding)
                    electricityProviders.Add(electricResourceBuilding.ElectricityProvider);
                if(buildingMapObject is IElectricBuildingCore electricBuilding)
                    electricBuildingsContainers.Add(electricBuilding.ElectricityContainer);
            }
            
            WireSystem.AddElectricityContainersRange(electricBuildingsContainers);
            WireSystem.AddElectricityProvidersRange(electricityProviders);
        }

        private void InitWireSystem(IEnumerable<BuildingMapObject> neighbourBuildings)
        {
            var wireObject = neighbourBuildings
                .FirstOrDefault(building => building is WirePoleMapBuilding wirePoleMapBuilding &&
                                            wirePoleMapBuilding.WirePoleBuildingCore != this) as WirePoleMapBuilding;
            
            if (!wireObject)
                WireSystem = new WireSystem(_updateService);
            else
            {
                WireSystem = wireObject.WireSystem;
                
                WireRopeFactory.Instance.SpawnRope(_ropePoint,wireObject.RopePoint);
            }
        }

        private IEnumerable<BuildingMapObject> FindNeighbourBuildings()
        {
            var neighbours = _mapCellsService.GetCellNeighboursWithVisitor<BuildingMapObject>(_cellPosition, Radius).ToList();
            foreach (var cellPositionTuple in neighbours)
            {
                yield return cellPositionTuple.Item1.CellVisitor as BuildingMapObject;
            }
        }

        public void Tick(float time)
        {
            
        }

        public void OnNeighbourUpdated(ICell cell, Vector2Int direction)
        {
            
        }
    }
}