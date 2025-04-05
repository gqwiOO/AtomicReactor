using System.Collections.Generic;
using _Project.Core.Services.UpdateService;
using UnityEngine;

namespace Core.Timer
{
    public class TimersProvider: MonoBehaviour, IUpdatable
    {
        public UpdateType UpdateType => UpdateType.Update;
        
        private readonly List<Timer> _timers = new();
        
        public void AddTimer(Timer timer)
        {
            _timers.Add(timer);
            timer.OnTimerEnded += RemoveTimer;
        }

        public void Tick() => TickTimers();

        private void RemoveTimer(object sender, TimerData e)
        {
            Timer timer = (Timer)sender;
            timer.OnTimerEnded -= RemoveTimer;
        }

        private void TickTimers()
        {
            _timers
                .FindAll(timer => timer.IsActive)
                .ForEach(timer => timer.Tick(Time.deltaTime));
        }
    }
}