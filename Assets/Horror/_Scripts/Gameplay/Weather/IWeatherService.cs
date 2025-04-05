using Core.Scripts.Debugging.Logging;
using Cysharp.Threading.Tasks;

namespace Gameplay.Weather
{
    public interface IWeatherService
    {
        UniTask SetWeather(WeatherType weatherType);
    }
}