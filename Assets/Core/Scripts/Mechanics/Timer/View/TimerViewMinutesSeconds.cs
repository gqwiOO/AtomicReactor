using System;

namespace Core.Timer
{
    public class TimerViewMinutesSeconds : BaseTimerView
    {
        protected override string ConvertTimerDataToString(TimerData data)
        {
            TimeSpan time = TimeSpan.FromSeconds(data.CurrentTime);
            string result = time.ToString(@"mm\:ss");
            return result;
        }
    }
}