using System;
using System.Collections.Generic;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.ItemPipeSystem
{
    public class ItemPipeSystem : IItemPipeSystem
    {
        private List<IItemPipe> _pipes = new();

        public IEnumerable<IItemPipe> Pipes => _pipes;
        public IList<(ICell cell, IItemExtractionSource source)> Sources { get; private set; } = new List<(ICell, IItemExtractionSource)>();
        public IList<(ICell cell, IItemInsertionTarget target)> Targets { get; private set; } = new List<(ICell, IItemInsertionTarget)>();

        private float _transferTimer;
        private const float TransferInterval = 0.5f;
        private const int TransferAmount = 1;
        private int _lastTickedFrame = -1;

        public void AddPipe(IItemPipe pipe) => _pipes.Add(pipe);

        public void Tick(float deltaTime)
        {
            if (Time.frameCount == _lastTickedFrame) return;
            _lastTickedFrame = Time.frameCount;

            _transferTimer += deltaTime;
            if (_transferTimer < TransferInterval) return;
            _transferTimer -= TransferInterval;

            TransferItems();
        }

        private void TransferItems()
        {
            foreach (var (sourceCell, source) in Sources)
            {
                if (!source.HasItemsForPipe()) continue;

                int itemId = source.GetExtractableItemId();
                if (itemId == -1) continue;
                
                bool hasDestinationForItem = HasDestinationForItem(sourceCell, source, itemId);
                
                if(!hasDestinationForItem) continue;

                foreach (var (targetCell, target) in Targets)
                {
                    if (targetCell == sourceCell) continue;
                    if (!target.CanInsertFromPipe(itemId, TransferAmount)) continue;

                    source.ExtractForPipe(itemId, TransferAmount);
                    target.InsertFromPipe(itemId, TransferAmount);
                    break;
                }
            }
        }

        private bool HasDestinationForItem(ICell sourceCell, IItemExtractionSource source, int itemId)
        {
            return true;
        }

        public void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            if (neighborCell.CellVisitor is IItemExtractionSource source)
                TryAddSource(neighborCell, source);

            if (neighborCell.CellVisitor is IItemInsertionTarget target)
                TryAddTarget(neighborCell, target);

            if (neighborCell.CellVisitor == null)
            {
                TryRemoveSource(neighborCell);
                TryRemoveTarget(neighborCell);
            }
        }

        private void TryAddSource(ICell cell, IItemExtractionSource source)
        {
            foreach (var entry in Sources)
                if (entry.cell == cell) return;
            Sources.Add((cell, source));
        }

        private void TryAddTarget(ICell cell, IItemInsertionTarget target)
        {
            foreach (var entry in Targets)
                if (entry.cell == cell) return;
            Targets.Add((cell, target));
        }

        private void TryRemoveSource(ICell cell)
        {
            for (int i = Sources.Count - 1; i >= 0; i--)
                if (Sources[i].cell == cell)
                    Sources.RemoveAt(i);
        }

        private void TryRemoveTarget(ICell cell)
        {
            for (int i = Targets.Count - 1; i >= 0; i--)
                if (Targets[i].cell == cell)
                    Targets.RemoveAt(i);
        }

        public IItemPipeSystem CollapseSystems(List<IItemPipeSystem> systems)
        {
            foreach (IItemPipeSystem system in systems)
            {
                foreach (IItemPipe pipe in system.Pipes)
                {
                    pipe.UpdateParentSystem(this);
                    _pipes.Add(pipe);
                }

                foreach (var (cell, source) in system.Sources)
                    TryAddSource(cell, source);

                foreach (var (cell, target) in system.Targets)
                    TryAddTarget(cell, target);

                system.Dispose();
            }

            return this;
        }

        public void Dispose()
        {
            _pipes = null;
            Sources = null;
            Targets = null;
        }
    }
}
