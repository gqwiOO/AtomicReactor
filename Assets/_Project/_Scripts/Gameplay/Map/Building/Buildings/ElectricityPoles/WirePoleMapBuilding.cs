using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using Gameplay.Map.Wires;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.ElectricityPoles
{
    public class WirePoleMapBuilding: BuildingMapObject
    {
        [field: SerializeField] public Transform RopePoint { get; private set; }
        
        private IMapCellsService _mapCellsService;

        public WirePoleBuildingCore WirePoleBuildingCore;

        public WireSystem WireSystem => WirePoleBuildingCore.WireSystem;

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }
        
        public override async UniTask Init(Vector2Int cellPosition)
        {
            WirePoleBuildingCore = new WirePoleBuildingCore(cellPosition, _mapCellsService,_updateService);
            WirePoleBuildingCore.Init(RopePoint);
            base.Init(cellPosition);
        }
        
        public override void Tick()
        {   
            
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
        }
    }
}