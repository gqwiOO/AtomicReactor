using Cysharp.Threading.Tasks;
using Raccoons.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Buildings.Screens
{
    public class ConfirmDestroyBuildingPopup : BaseScreen
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private UniTaskCompletionSource<bool> _tcs;

        protected override void Awake()
        {
            base.Awake();
            confirmButton.onClick.AddListener(OnConfirmClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            confirmButton.onClick.RemoveListener(OnConfirmClicked);
            cancelButton.onClick.RemoveListener(OnCancelClicked);
        }

        public async UniTask<bool> ShowAndAwaitResult()
        {
            _tcs = new UniTaskCompletionSource<bool>();
            gameObject.SetActive(true);
            return await _tcs.Task;
        }

        private void OnConfirmClicked()
        {
            gameObject.SetActive(false);
            _tcs?.TrySetResult(true);
        }

        private void OnCancelClicked()
        {
            gameObject.SetActive(false);
            _tcs?.TrySetResult(false);
        }
    }
}
