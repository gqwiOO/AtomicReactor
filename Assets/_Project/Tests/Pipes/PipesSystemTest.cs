using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [UnityTest]
        public IEnumerator PipesDividingtest()
        {
            PipeSystem PipeSystem = new FluidPipeSystem();
            IFluidProvider fluidProvider = new FluidProvider();
            PipeSystem.AddFluidProvider(fluidProvider);
            
            for (int x = 0; x < 5; x++)
            {
                IPipe pipe = Object.Instantiate(Resources.Load<GameObject>("Pipe")).GetComponent<FluidPipe>();
                PipeSystem.AddPipe(pipe);
            }
            
            fluidProvider.AddFluid(FluidType.Water,amount: 1000);
            
            Assert.AreEqual(200,PipeSystem.Pipes.First().FillValue);
            yield return null;
        }
    }
}