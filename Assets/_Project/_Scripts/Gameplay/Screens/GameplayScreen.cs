using Gameplay.Buildings.Screens;
using Gameplay.UI.Services;
using Raccoons.UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Screens
{
    public class GameplayScreen: BaseScreen
    {
        [SerializeField] private Button showSelectBuildingToBuildButton;
        private IGameplayScreensService _gameplayScreensService;

        [Inject]
        private void Construct(IGameplayScreensService gameplayScreensService)
        {
            _gameplayScreensService = gameplayScreensService;
        }
        private void Start()
        {
            showSelectBuildingToBuildButton.onClick.AddListener(SelectBuildingButton_OnClicked);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            showSelectBuildingToBuildButton.onClick.RemoveListener(SelectBuildingButton_OnClicked);
        }

        private void SelectBuildingButton_OnClicked()
        {
            _gameplayScreensService.ShowSelectBuildingScreen();
        }
    }
}