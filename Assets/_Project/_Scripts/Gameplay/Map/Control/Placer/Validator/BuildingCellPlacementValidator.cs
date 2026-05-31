using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Validator
{
    public class BuildingCellPlacementValidator : IBuildingCellPlacementValidator
    {
        private IMapCellsService _mapCellsService;

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }
        
        public bool CanBePlaced(IBuildingSettingsData buildingSettingsData, Vector2Int cellPosition)
        {
            List<bool> conditions = new List<bool>();
            ICell cell = _mapCellsService.GetCell(cellPosition);
            conditions.Add(cell.CellVisitor == null);
            if (buildingSettingsData.BuildingPlacementSettings == null)
                return true;
            foreach (BuildingPlacementSettingData keyValuePair in buildingSettingsData.BuildingPlacementSettings.CellTypeValidation)
            {
                if (keyValuePair.BuildingPlacementValidationType == BuildingPlacementValidationType.AnyNeighbour)
                {
                    IEnumerable<ICell> neighbours = _mapCellsService.GetCellNeighbours(cellPosition);
                    bool isValid = neighbours.Any(item => keyValuePair.CellTypes.Contains(item.CellType));
                    conditions.Add(isValid);
                }

                if (keyValuePair.BuildingPlacementValidationType == BuildingPlacementValidationType.BuildingCell)
                {
                    bool isValid = keyValuePair.CellTypes.Any(item => item == cell.CellType);
                    conditions.Add(isValid);
                }
            }

            return conditions.All(item => item);
        }
    }
}