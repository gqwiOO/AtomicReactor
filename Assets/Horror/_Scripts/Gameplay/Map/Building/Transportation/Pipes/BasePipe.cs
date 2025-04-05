using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building;
using Gameplay.Map.Building.Items.Data;
using Gameplay.Map.Cell;
using Gameplay.Map.CellsService;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public abstract class BasePipe : BuildingMapObject,IPipe
    {
        [field: SerializeField] public FluidType FluidType { get; set; }
        [field: SerializeField] public float FillValue { get; set; }
        
        [field: SerializeField] public ICell Cell { get; set; }
        
        [SerializeField] private PipeView pipeView;
        
        private List<IPipe> _connectedPipes = new ();
        
        protected IMapCellsService _mapCellsService;
        private Dictionary<Vector2Int,ICell> _neighbours;
        private Dictionary<Vector2Int,IPipe> _neighboursPipes = new Dictionary<Vector2Int, IPipe>();
        private List<IPipe> _availableToConnectionsPipes = new List<IPipe>();
        private Vector4 _pipeConnections;
        
        public IPipeSystem ParentPipeSystem { get; private set; }
        public IEnumerable<IPipe> ConnectedPipes => _connectedPipes;
        public int ConnectedPipesCount => _connectedPipes.Count;
        
        public bool ConnectedTo(IPipe pipe) => _connectedPipes.Contains(pipe);

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }

        public override async UniTask Init(Vector2Int cellPosition)
        {
            await base.Init(cellPosition);
            await InitPipeSystem();
        }

        public override void Tick()
        {
        }

        private async UniTask InitPipeSystem()
        {
            HashSet<IPipeSystem> result = await TryFindPipeSystem();

            IPipeSystem pipeSystem = null;
            if (result.Count > 1)
            {
                var collapsedSystems = new List<IPipeSystem>(result);
                collapsedSystems.RemoveAt(0);
                pipeSystem = result.First().CollapseSystems(collapsedSystems);
            }
            else if (result.Count == 1)
            {
                pipeSystem = result.First();
            }
            
            if (pipeSystem != null)
            {
                SetPipeViewState();
                UpdateParentSystem(pipeSystem);
            }
            else
            {
                pipeSystem = new FluidPipeSystem();
                UpdateParentSystem(pipeSystem);
            }
            pipeSystem.AddPipe(this);
        }

        private void SetPipeViewState()
        {
            UpdatePipeState(true, true);
            RotateTowardDirection(_pipeConnections);
        }

        private void UpdatePipeState(bool notifyNeighbours, bool checkNeighboursConnectionsCount)
        {
            _pipeConnections = new Vector4();
            _connectedPipes.Clear();
            
            
            foreach(var neighbour in _neighbours)
            {
                if (neighbour.Value.CellVisitor is IPipe pipe)
                {
                    _neighboursPipes.TryAdd(neighbour.Value.Position, pipe);
                    
                    var pos = neighbour.Value.Position - CellPosition;
                    if (pos == Vector2Int.left)
                    {
                        _pipeConnections.w = 1;
                        _connectedPipes.Add(pipe);
                    }

                    if (pos == Vector2Int.right)
                    {
                        _pipeConnections.y = 1;
                        _connectedPipes.Add(pipe);
                    }

                    if (pos == Vector2Int.up)
                    {
                        _pipeConnections.x = 1;
                        _connectedPipes.Add(pipe);
                    }

                    if (pos == Vector2Int.down)
                    {
                        _pipeConnections.z = 1;
                        _connectedPipes.Add(pipe);
                    }
                }
            }
            
            
            if (notifyNeighbours)
            {
                foreach (var neighbour in _neighbours)
                {
                    if (neighbour.Value.CellVisitor is IPipe pipe)
                    {
                        pipe.NotifyToChangeRotationState();
                    }
                }
            }

           
        }

        private async UniTask<HashSet<IPipeSystem>> TryFindPipeSystem()
        {
            HashSet<IPipeSystem> systems = new();
            _neighbours = GetNeighbours();
            foreach (var neighbour in _neighbours)
                if (neighbour.Value.CellVisitor is IPipe pipe)
                    systems.Add(pipe.ParentPipeSystem);
            
            return systems;
        }

        private Dictionary<Vector2Int,ICell> GetNeighbours()
        {
            var cellNeighboursWithPositions = _mapCellsService.GetCellNeighboursWithPositionsDictionary(CellPosition);
            Dictionary<Vector2Int,ICell> neighbours = cellNeighboursWithPositions;
            return neighbours;
        }

        public void AddFluid(float amount) => FillValue += amount;

        public void UpdateParentSystem(IPipeSystem pipeSystem)
        {
            ParentPipeSystem = pipeSystem;
            
#if UNITY_EDITOR
            name = $"Pipe {ParentPipeSystem.GetHashCode()}";
#endif
        }

        public void RotateTowardDirection(Vector4 neighbours) => pipeView.SetState(neighbours);

        public void NotifyToChangeRotationState()
        {
            UpdatePipeState(false, false);
            RotateTowardDirection(_pipeConnections);
        }
    }
}