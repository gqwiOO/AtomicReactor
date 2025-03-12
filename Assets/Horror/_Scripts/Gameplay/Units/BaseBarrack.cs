using System;
using Gameplay.Map.Building;
using Gameplay.Map.CellsService;
using Gameplay.Units.Factory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Units
{
    public abstract class BaseBarrack: BuildingMapObject
    {
        [FormerlySerializedAs("_unit")] [SerializeField]
        protected BaseBarrackBarackUnit barackUnit;

        [SerializeField]
        protected int unitsCount;

        private IUnitsFactory _unitsFactory;
        private IMapCellsService _mapCellsService;

        [Inject]
        private void Construct(IUnitsFactory unitsFactory, IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
            _unitsFactory = unitsFactory;
        }
        
        public IBarackUnit CreateUnit()
        {
            IBarackUnit result = _unitsFactory.SpawnUnit(barackUnit.Key,
                _mapCellsService.GetCell(CellPosition + new Vector2Int(0, -1)).WorldPosition);

            result.OnReturnedToBarrack += Unit_OnReturnedToBarrack;
            
            return result;
        }

        protected abstract void Unit_OnReturnedToBarrack();
    }
}