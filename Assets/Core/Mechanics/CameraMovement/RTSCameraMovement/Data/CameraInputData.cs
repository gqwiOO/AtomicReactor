using System;
using UnityEngine;

namespace Core.CameraMovement.RTSCameraMovement
{
    [Serializable]
    public class CameraInputData
    {
        public Transform Pivot;
        public float Smoothness;
        public float MouseSensitivity;
        public float KeyboardSensitivity;
    }
}