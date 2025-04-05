using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public class FluidPipe : BasePipe
    {
        public override async UniTask Init(Vector2Int cellPosition)
        {
            await  base.Init(cellPosition);
            
            IEnumerable<Tuple<ICell, Vector2Int>> neighbours = _mapCellsService.GetCellNeighboursWithPositions(cellPosition);
            
            foreach (var (cell, vector2Int) in neighbours)
            {
                ParentPipeSystem.NotifyAboutNeighborUpdated(cell, vector2Int);
            }
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            ParentPipeSystem.NotifyAboutNeighborUpdated(neighborCell, direction);
        }
    }
}