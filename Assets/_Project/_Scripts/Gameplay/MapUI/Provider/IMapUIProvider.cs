using Gameplay.Map.Building;
using Gameplay.MapUI.Views;

namespace Gameplay.MapUI
{
    public interface IMapUIProvider
    {
        ElectricFurnaceView GetElectricFurnaceView();

        IMapUIObjectView GetMapUIView(string key);
    }
}