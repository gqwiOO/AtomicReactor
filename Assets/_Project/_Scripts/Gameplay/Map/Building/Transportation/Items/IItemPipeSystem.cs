using System;
using System.Collections.Generic;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.ItemPipeSystem
{
    public interface IItemPipeSystem : IDisposable
    {
        int Key { get; }
        IEnumerable<IItemPipe> Pipes { get; }
        IList<(ICell cell, IItemExtractionSource source)> Sources { get; }
        IList<(ICell cell, IItemInsertionTarget target)> Targets { get; }

        void AddPipe(IItemPipe pipe);
        void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction);
        IItemPipeSystem CollapseSystems(List<IItemPipeSystem> systems);
        void Tick(float deltaTime);
    }
}
