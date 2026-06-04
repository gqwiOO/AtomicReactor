using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using Gameplay.Transportation.WaterPipeSystem;
using UnityEngine;
using Zenject;

namespace Gameplay.Transportation.ItemPipeSystem
{
    public class ItemPipe : BuildingMapObject, IItemPipe
    {
        [SerializeField] private PipeView pipeView;

        private IMapCellsService _mapCellsService;
        private Dictionary<Vector2Int, ICell> _neighbours;
        private Vector4 _pipeConnections;

        public IItemPipeSystem ParentPipeSystem { get; private set; }

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }

        public override async UniTask Init(Vector2Int cellPosition)
        {
            await base.Init(cellPosition);
            await InitPipeSystem();

            foreach (var (cell, direction) in _mapCellsService.GetCellNeighboursWithPositions(cellPosition))
                ParentPipeSystem.NotifyAboutNeighborUpdated(cell, direction);
        }

        private async UniTask InitPipeSystem()
        {
            _neighbours = _mapCellsService.GetCellNeighboursWithPositionsDictionary(CellPosition);

            HashSet<IItemPipeSystem> foundSystems = new();
            foreach (var (_, cell) in _neighbours)
                if (cell.CellVisitor is IItemPipe pipe)
                    foundSystems.Add(pipe.ParentPipeSystem);

            IItemPipeSystem system;
            if (foundSystems.Count > 1)
            {
                var list = new List<IItemPipeSystem>(foundSystems);
                var primary = list[0];
                list.RemoveAt(0);
                system = primary.CollapseSystems(list);
            }
            else if (foundSystems.Count == 1)
            {
                system = foundSystems.First();
            }
            else
            {
                system = new ItemPipeSystem();
            }

            UpdateParentSystem(system);
            system.AddPipe(this);

            UpdatePipeConnections(notifyNeighbours: true);
        }

        public override void Tick()
        {
            ParentPipeSystem?.Tick(Time.deltaTime);
        }

        public override void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction)
        {
            _neighbours ??= new Dictionary<Vector2Int, ICell>();
            _neighbours[neighborCell.Position] = neighborCell;

            ParentPipeSystem?.NotifyAboutNeighborUpdated(neighborCell, direction);
            UpdatePipeConnections(notifyNeighbours: false);
        }

        private void UpdatePipeConnections(bool notifyNeighbours)
        {
            _pipeConnections = Vector4.zero;

            if (_neighbours == null) return;

            foreach (var (_, cell) in _neighbours)
            {
                if (cell.CellVisitor is not IItemPipe) continue;

                var relativePos = cell.Position - CellPosition;
                if (relativePos == Vector2Int.up)    _pipeConnections.x = 1;
                if (relativePos == Vector2Int.right)  _pipeConnections.y = 1;
                if (relativePos == Vector2Int.down)   _pipeConnections.z = 1;
                if (relativePos == Vector2Int.left)   _pipeConnections.w = 1;
            }

            RotateTowardDirection(_pipeConnections);

            if (!notifyNeighbours) return;
            foreach (var (_, cell) in _neighbours)
                if (cell.CellVisitor is IItemPipe pipe)
                    pipe.NotifyToChangeRotationState();
        }

        public void UpdateParentSystem(IItemPipeSystem system)
        {
            ParentPipeSystem = system;
            gameObject.name = $"{GetType().Name} [System: {system.Key}]";
        }

        public void RotateTowardDirection(Vector4 neighbourStates)
        {
            pipeView?.SetState(neighbourStates);
        }

        public void NotifyToChangeRotationState()
        {
            UpdatePipeConnections(notifyNeighbours: false);
        }

        public virtual bool CanTransport(int itemId) => true;
    }
}
