using System;
using Gameplay.Map.Creator;
using UnityEngine;

namespace Gameplay.Map.Cell
{
    public interface ICell
    {
        Vector2Int Position { get; }
        
        Vector3 WorldPosition { get; }

        ICellVisitor CellVisitor { get; }
        CellType CellType { get; }
        BiomeType Biome { get; }

        event Action<ICell> OnUpdated;

        void NotifyAboutNeighbourUpdated(ICell cell);

        void SetVisitor(ICellVisitor cellVisitor);
    }

    public interface ICellVisitor
    {
        void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int side);

        event Action OnUpdated;
    }
    
}