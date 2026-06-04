using UnityEngine;

namespace Gameplay.Transportation.ItemPipeSystem
{
    public interface IItemPipe
    {
        IItemPipeSystem ParentPipeSystem { get; }
        Vector2Int CellPosition { get; }
        bool CanTransport(int itemId);
        void UpdateParentSystem(IItemPipeSystem system);
        void NotifyToChangeRotationState();
        void RotateTowardDirection(Vector4 neighbourStates);
    }
}
