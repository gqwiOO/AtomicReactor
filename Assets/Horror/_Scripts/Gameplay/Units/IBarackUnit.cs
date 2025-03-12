using System;
using Core.Scripts.StateMachine.States.Animating;
using Gameplay.Map.Nature;
using Gameplay.Units.Lumberkack.States;
using Unity.VisualScripting;

namespace Gameplay.Units
{
    public interface IBarackUnit
    {
        string Key {get;}

        void StartWorking();

        void SetBarrack(BaseBarrack barrack);

        event Action OnReturnedToBarrack;
    }
}