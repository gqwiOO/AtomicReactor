using System.Collections.Generic;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public interface IPipe
    {
        IPipeSystem ParentPipeSystem { get; }
        FluidType FluidType { get; }
        float FillValue { get; }
        ICell Cell { get; }

        IEnumerable<IPipe> ConnectedPipes { get; }
        
        bool ConnectedTo(IPipe pipe);
        int ConnectedPipesCount { get;}
        void AddFluid(float amount);
        void UpdateParentSystem(IPipeSystem pipeSystem);
        void RotateTowardDirection(Vector4 neighboursStates);
        void NotifyToChangeRotationState();
    }

    public enum FluidType
    {
        None = 0,
        Water = 1,
    }
}