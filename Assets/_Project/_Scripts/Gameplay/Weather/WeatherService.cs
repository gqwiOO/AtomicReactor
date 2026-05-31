using System;
using System.Collections.Generic;
using Core.Scripts.Debugging;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Weather
{
    public class WeatherService : SerializedMonoBehaviour, IWeatherService
    {
        [SerializeField]
        private Dictionary<WeatherType,BaseWeatherObject> weatherObjects;

        private BaseWeatherObject _currentBaseWeatherObject;

        public WeatherType CurrentWeather { get; private set; }

        private void Start()
        {
            SetWeather(WeatherType.Rainy);
        }

        public async UniTask SetWeather(WeatherType weatherType)
        {
            weatherObjects.TryGetValue(weatherType, out BaseWeatherObject weatherObject);

            if (weatherObject == null)
                Debugging.Error(this, $"Cant find object of weather : {weatherType.ToString()}");
            
            if (_currentBaseWeatherObject != null)
                await _currentBaseWeatherObject.Exit();
            await weatherObject.Enter();
            
            _currentBaseWeatherObject = weatherObject;
            CurrentWeather = weatherType;
            
        }
    }
}