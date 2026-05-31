using UnityEngine;

namespace Gameplay.Transportation.ItemPipeSystem
{
    public interface IItemPipe
    {
        IItemPipeSystem ParentPipeSystem { get; }
        void UpdateParentSystem(IItemPipeSystem system);
        void NotifyToChangeRotationState();
        void RotateTowardDirection(Vector4 neighbourStates);
    }
}
