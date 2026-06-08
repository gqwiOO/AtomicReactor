using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Cell;
using Gameplay.Transportation.WaterPipeSystem;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Horror.Tests.Pipes
{
    public class PipesSystemTest
    {
        private class FakeCell : ICell
        {
            public Vector2Int Position => Vector2Int.zero;
            public Vector3 WorldPosition => Vector3.zero;
            public ICellVisitor CellVisitor { get; private set; }
            public CellType CellType => CellType.None;
            public event Action<ICell> OnUpdated;
            public void NotifyAboutNeighbourUpdated(ICell cell) { }
            public void SetVisitor(ICellVisitor visitor) => CellVisitor = visitor;
        }

        private class FakeFluidSource : ICellVisitor, IFluidExtractionSource
        {
            public IFluidContainer ExtractionFluidContainer { get; }
            public FakeFluidSource(IFluidContainer provider) => ExtractionFluidContainer = provider;
            public void NotifyAboutNeighborUpdated(ICell neighborCell, Vector2Int side) { }
            public event Action OnUpdated;
        }

        [UnityTest]
        public IEnumerator PipesDividingtest()
        {
            FluidPipeSystem PipeSystem = new FluidPipeSystem();
            IFluidContainer fluidProvider = new FluidContainer();
            var fakeCell = new FakeCell();
            fakeCell.SetVisitor(new FakeFluidSource(fluidProvider));
            PipeSystem.NotifyAboutNeighborUpdated(fakeCell, Vector2Int.zero);
            
            for (int x = 0; x < 5; x++)
            {
                BasePipe pipe = Object.Instantiate(Resources.Load<GameObject>("Pipe")).GetComponent<FluidPipe>();
                PipeSystem.AddPipe(pipe);
            }
            
            fluidProvider.AddFluid(FluidType.Water,amount: 1000);
            
            Assert.AreEqual(200,PipeSystem.Pipes.First().FillValue);
            yield return null;
        }
    }
}