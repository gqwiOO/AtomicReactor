using System;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Map.Nature
{
    public class TreeMapObject: MonoBehaviour, ICellVisitor
    {
        public void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int side)
        {
            
        }

        public event Action OnUpdated;
    }
}