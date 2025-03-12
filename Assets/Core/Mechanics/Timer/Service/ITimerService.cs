namespace Core.Timer.Service
{
    public interface ITimerService
    {
        Timer CreateTimer(TimerData timerData);
    }
}