using System;
using Core.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using Gameplay.Map.CellsService;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public abstract class BaseBarrackBarackUnit : MonoBehaviour, IBarackUnit
    {
        protected const string RUNNING_ANIMATION_KEY = "Running";
        
        [SerializeField]
        protected BaseStateMachine _stateMachine;

        [SerializeField]
        protected float _speed;
        
        
        protected IMapCellsService _mapCellsService;
        protected BaseBarrack _barrack;

        public abstract event Action OnReturnedToBarrack;

        [field: SerializeField]
        public string Key { get; private set; }

        [Inject]
        private void Construct(IMapCellsService mapCellsService)
        {
            _mapCellsService = mapCellsService;
        }

        public void SetBarrack(BaseBarrack barrack)
        {
            _barrack = barrack;
        }

        public abstract void StartWorking();

        protected abstract UniTask WorkProcess();
    }
}