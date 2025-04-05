using System;

namespace Core.Timer
{
    public class TimerViewHoursMinutesSeconds : BaseTimerView
    {
        protected override string ConvertTimerDataToString(TimerData data)
        {
            TimeSpan time = TimeSpan.FromSeconds(data.CurrentTime);
            string result = time.ToString(@"hh\:mm\:ss");
            return result;
        }
    }
}