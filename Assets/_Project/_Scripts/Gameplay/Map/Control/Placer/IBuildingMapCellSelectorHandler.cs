namespace Gameplay.Map.Building.Placer
{
    public interface IBuildingMapCellSelectorHandler
    {
        void TurnOn(string key);
        void TurnOff();
    }
}