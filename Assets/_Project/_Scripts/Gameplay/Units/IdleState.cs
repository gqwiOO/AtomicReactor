using Core.Scripts.StateMachine.States;
using Core.Scripts.StateMachine.States.Animating;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Units
{
    public class IdleState: MonoBehaviour, IPayloadState<float>
    {

        private const string IDLE_ANIMATION_KEY = "Idle";
        [SerializeField]
        private BaseAnimationState animationState;

        private AnimationStateData _animationStateData = new(IDLE_ANIMATION_KEY);
        public async UniTask Enter(float duration = 5)
        {
            animationState.Enter(_animationStateData).Forget();
            await UniTask.WaitForSeconds(duration);
        }

        public async UniTask Exit()
        {
            
        }
    }
}