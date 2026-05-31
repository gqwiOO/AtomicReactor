
using System;
using UnityEngine;

namespace Gameplay.Map.Building.Validator
{
    public interface IBuildingCellPlacementValidator
    {
        bool CanBePlaced(IBuildingSettingsData buildingSettingsData, Vector2Int cellPosition);
    }
}