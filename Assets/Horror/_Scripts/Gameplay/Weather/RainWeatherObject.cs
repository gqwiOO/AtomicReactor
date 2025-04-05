using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Gameplay.Weather
{
    public class RainWeatherObject : BaseWeatherObject
    {
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private float transitionDuration;
        [SerializeField] private float endVolume;
        
        public override WeatherType WeatherType { get; }
        public override async UniTask Enter()
        {
            audioSource.volume = 0;
            audioSource.time = 0;
            audioSource.Play();

            await TweenSound();
        }

        private async UniTask TweenSound()
        {
            await DOTween.To(() => audioSource.volume, x => audioSource.volume = x, endVolume, transitionDuration);
        }

        public override async UniTask Exit()
        {
            await DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, transitionDuration);
        }
    }
}