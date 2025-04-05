using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Weather
{
    public abstract class BaseWeatherObject: MonoBehaviour
    {
        public abstract WeatherType WeatherType { get; }

        public abstract UniTask Enter();
        public abstract UniTask Exit();
    }
}