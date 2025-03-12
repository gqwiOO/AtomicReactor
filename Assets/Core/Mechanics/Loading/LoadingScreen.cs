using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Raccoons.UI.Screens;
using UnityEngine;

namespace Core.Mechanics.Loading
{
    public class LoadingScreen: BaseScreen
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;
        
        public override async Task Open(CancellationToken cancellationToken = default)
        {
            await base.Open(cancellationToken);
            _canvasGroup.alpha = 0;
            await _canvasGroup.DOFade(1f,_fadeDuration).AsyncWaitForKill();
        }
    }
}