using System;
using System.Collections.Generic;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.ItemPipeSystem
{
    public class ItemPipeSystem : IItemPipeSystem
    {
        private static readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        private List<IItemPipe> _pipes = new();
        private Dictionary<Vector2Int, IItemPipe> _pipesByPosition = new();

        public int Key { get; private set; }

        public ItemPipeSystem()
        {
            Key = GetHashCode();
        }

        public IEnumerable<IItemPipe> Pipes => _pipes;
        public IList<(ICell cell, IItemExtractionSource source)> Sources { get; private set; } = new List<(ICell, IItemExtractionSource)>();
        public IList<(ICell cell, IItemInsertionTarget target)> Targets { get; private set; } = new List<(ICell, IItemInsertionTarget)>();

        private float _transferTimer;
        private const float TransferInterval = 0.5f;
        private const int TransferAmount = 1;
        private int _lastTickedFrame = -1;
        private int _roundRobinIndex;

        public void AddPipe(IItemPipe pipe)
        {
            _pipes.Add(pipe);
            _pipesByPosition[pipe.CellPosition] = pipe;
        }

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

                int count = Targets.Count;
                if (count == 0) continue;
                if (_roundRobinIndex >= count) _roundRobinIndex = 0;

                for (int i = 0; i < count; i++)
                {
                    int idx = (_roundRobinIndex + i) % count;
                    var (targetCell, target) = Targets[idx];
                    _roundRobinIndex = (idx + 1) % count;

                    if (targetCell == sourceCell) continue;
                    if (!target.CanInsertFromPipe(itemId, TransferAmount)) continue;
                    if (!CanReachTarget(sourceCell, targetCell, itemId)) continue;

                    source.ExtractForPipe(itemId, TransferAmount);
                    target.InsertFromPipe(itemId, TransferAmount);
                    break;
                }
            }
        }

        private bool HasDestinationForItem(ICell sourceCell, IItemExtractionSource source, int itemId)
        {
            foreach (var (targetCell, _) in Targets)
                if (targetCell != sourceCell && CanReachTarget(sourceCell, targetCell, itemId))
                    return true;
            return false;
        }

        private bool CanReachTarget(ICell sourceCell, ICell targetCell, int itemId)
        {
            var visited = new HashSet<Vector2Int> { sourceCell.Position };
            var queue = new Queue<Vector2Int>();
            queue.Enqueue(sourceCell.Position);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var dir in Directions)
                {
                    var neighbor = current + dir;
                    if (neighbor == targetCell.Position) return true;
                    if (_pipesByPosition.TryGetValue(neighbor, out var pipe) && pipe.CanTransport(itemId) && visited.Add(neighbor))
                        queue.Enqueue(neighbor);
                }
            }
            return false;
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
                    _pipesByPosition[pipe.CellPosition] = pipe;
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
            _pipesByPosition = null;
            Sources = null;
            Targets = null;
        }
    }
}
