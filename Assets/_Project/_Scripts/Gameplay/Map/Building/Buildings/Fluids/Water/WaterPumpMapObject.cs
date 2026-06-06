using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using Gameplay.Transportation.WaterPipeSystem;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Fluids
{
    
    //todo: remove extractionfluidProvider and keep water in self container 
    public class WaterPumpMapObject: BuildingMapObject, IFluidExtractionSource
    {
        private IMapCellsService _mapCellsService;
        
        public IFluidProvider ExtractionFluidProvider { get; private set; }

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }
        
        public override async UniTask Init(Vector2Int cellPosition)
        {
            ExtractionFluidProvider = new FluidProvider(FluidType.Water, 1000);
            await base.Init(cellPosition);
            InitRotation();

        }

        private void InitRotation()
        {
            var neighbours = _mapCellsService.GetCellsWithTypes(CellPosition);
            ICell waterCell = null;
            foreach (KeyValuePair<ICell, CellType> valuePair in neighbours)    
            {
                if (valuePair.Value == CellType.Water)
                {
                    waterCell = valuePair.Key;
                }
            }
            
            Vector2Int direction = waterCell.Position - CellPosition;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = rotation;
        }

        public override void Tick()
        {
            ExtractionFluidProvider.AddFluid(FluidType.Water, 1 * Time.deltaTime);
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            
        }
    }
}