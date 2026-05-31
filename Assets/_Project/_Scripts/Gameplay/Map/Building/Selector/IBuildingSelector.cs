using System;

namespace Gameplay.Map.Building.Selector
{
    public interface IBuildingSelector
    {
        BuildingMapObject SelectedBuilding { get; }
        bool IsSelectionLocked { get; set; }
        void Deselect();
        event Action<BuildingMapObject> OnSelected;
        event Action OnDeselected;
    }
}
