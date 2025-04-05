using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Core.Services.UpdateService;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Building.SettingsProvider;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building
{
    public abstract class BuildingMapObject: MonoBehaviour, IUpdatable, ICellVisitor
    {
        [SerializeField] 
        private Collider triggerCollider;

        [SerializeField] private List<MeshRenderer> meshRenderer;
        
        protected IUpdateService _updateService;

        protected IBuildingsSettingsProvider _buildingsSettingsProvider;
        
        private Material _material;

        [field: SerializeField]
        public Material BaseMaterial { get; private set; }

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

        public async Task StartBuilding(Vector2Int cellPosition)
        {
            // TODO:  Wait until built
            await Init(cellPosition);
            EnableTriggerCollider();
        }
        
        public void SetMaterial(Material material)
        {
            _material = material;
            meshRenderer.ForEach(item => item.material = material);
        }

        public void SetMaterialColor(Color color)
        {
            _material.color = color;
        }

        public virtual async UniTask Init(Vector2Int cellPosition)
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