using Core.Scripts.StateMachine.States;
using Core.Scripts.StateMachine.States.Animating;
using Cysharp.Threading.Tasks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Units.Lumberkack.States
{
    public class ChopState: MonoBehaviour, IPayloadState<ICell>
    {
        private const string ANIMATION_CHOPPING = "Chopping";
        
        [SerializeField] 
        private BaseAnimationState animationState;
        
        public async UniTask Enter(ICell duration)
        {
            await animationState.Enter(new AnimationStateData(ANIMATION_CHOPPING));
        }

        public async UniTask Exit()
        {
        }
    }
}