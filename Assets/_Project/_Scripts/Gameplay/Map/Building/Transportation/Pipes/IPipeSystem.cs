using System;
using System.Collections.Generic;
using Gameplay.Map.Building.Fluids.Tanks;
using Gameplay.Map.Cell;
using UnityEngine;

namespace Gameplay.Transportation.WaterPipeSystem
{
    public interface IPipeSystem: IDisposable
    {
        public int Key { get; }

        Dictionary<ICell, IFluidContainer> FluidExtractionContainers { get; }
        IEnumerable<BasePipe> Pipes { get; }

        IFloatContainer FloatContainer { get; }

        Dictionary<ICell, IFluidContainer> FluidInsertionContainers { get;}
        float CurrentValue { get; }

        void AddPipe(BasePipe pipe);
        void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int direction);
        IPipeSystem CollapseSystems(params IPipeSystem[] pipes);
        IPipeSystem CollapseSystems(List<IPipeSystem> pipes);
    }
}
