using System;
using _Project.Core.Services.UpdateService;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Cell;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Map.Building
{
    public abstract class BuildingMapObject: MonoBehaviour, IUpdatable, ICellVisitor
    {
        [SerializeField] 
        private Collider triggerCollider;
        
        protected IUpdateService _updateService;
        
        protected IBuildingsSettingsProvider _buildingsSettingsProvider;

        [field: SerializeField] 
        public string Key { get; private set; }


        public UpdateType UpdateType => UpdateType.Update;

        public bool IsWorking { get; protected set; }
        public Vector2Int CellPosition { get; private set; }

        public event Action OnUpdated;


        [Inject]
        private void Construct(IUpdateService updateService, IBuildingsSettingsProvider buildingsSettingsProvider)
        {
            _buildingsSettingsProvider = buildingsSettingsProvider;
            _updateService = updateService;
        }

        public void StartBuilding(Vector2Int cellPosition)
        {
            // TODO:  Wait until built
            Init(cellPosition);
            EnableTriggerCollider();
        }

        public virtual void Init(Vector2Int cellPosition)
        {
            CellPosition = cellPosition;
            IsWorking = true;
            _updateService.Add(this);
            OnUpdated?.Invoke();
        }

        protected void TriggerUpdate() => OnUpdated?.Invoke();

        public void DisableTriggerCollider() => triggerCollider.enabled = false;

        private void EnableTriggerCollider() => triggerCollider.enabled = true;
        public abstract void Tick();
        public abstract void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction);
    }
}