using UnityEngine;

namespace Core.CameraMovement.RTSCameraMovement
{
    public interface ICameraMovementHandler
    {
        void Move(Vector3 delta);

    }
    
    public class CameraMovementHandler : BaseCameraMovementHandler
    {
        protected override Vector3 GetTargetPosition(Vector3 delta)
        {
            var result = _cameraInputData.Pivot.position + delta;
            return result;
        }

        public CameraMovementHandler(CameraInputData cameraInputData) : base(cameraInputData)
        {
        }
    }
}