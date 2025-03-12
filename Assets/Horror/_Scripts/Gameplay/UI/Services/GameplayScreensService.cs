using System.Threading.Tasks;
using Gameplay.Buildings.Screens;
using Raccoons.UI.Screens;

namespace Gameplay.UI.Services
{
    public class GameplayScreensService : BaseScreenManager, IGameplayScreensService
    {
        public async Task ShowSelectBuildingScreen()
        {
            var screen = GetScreen<SelectBuildingScreen>();
            await screen.Open();
        }
    }
}