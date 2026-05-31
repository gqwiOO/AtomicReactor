using System;
using Core.Scripts.StateMachine.States.Moving;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using Gameplay.Map.Nature;
using Gameplay.Units.Lumberkack.States;
using UnityEngine;

namespace Gameplay.Units
{
    public class Lumberjack : BaseBarrackBarackUnit
    {
        [SerializeField] private float treeChopDuration;
        
        private ICell _currentTargetCell;

        // private void StartChopping()
        // {
        //     _stateMachine.Enter<ChopState,AnimationStateData>(new AnimationStateData(ANIMATION_CHOPPING));
        // }
        //
        // private void EndChopping()
        // {
        //     _stateMachine.Enter<ChopState,AnimationStateData>(new AnimationStateData(ANIMATION_CHOPPING));
        // }
        
        public override void StartWorking()
        {
            WorkProcess().Forget();
        }

        public override event Action OnReturnedToBarrack;

        protected override async UniTask WorkProcess()
        {
            MovingStateData movingToBarrackStateData =
                new MovingStateData(_speed, RUNNING_ANIMATION_KEY, _barrack.transform.position);
            while (true)
            {
                _currentTargetCell =
                    _mapCellsService.GetClosestEmptyCellWithNeighbourCellVisitor(out TreeMapObject tree, out ICell visitorCell, _barrack.CellPosition);

                MovingStateData movingStateData =
                    new MovingStateData(_speed, RUNNING_ANIMATION_KEY, _currentTargetCell.WorldPosition);
                await _stateMachine.Enter<MovingState, MovingStateData>(movingStateData);

                await _stateMachine.Enter<ChopState, ICell>(_currentTargetCell);
                
                Destroy(tree.gameObject);
                visitorCell.SetVisitor(null);
                
                await _stateMachine.Enter<IdleState, float>(1);
                await _stateMachine.Enter<MovingState, MovingStateData>(movingToBarrackStateData);
                OnReturnedToBarrack?.Invoke();
                await _stateMachine.Enter<IdleState, float>(1);
            }
        }
    }
}