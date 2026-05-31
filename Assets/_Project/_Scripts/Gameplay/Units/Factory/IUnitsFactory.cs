using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Units.Factory
{
    public interface IUnitsFactory
    {
        IBarackUnit SpawnUnit(string unitKey, Vector3 position);
    }
}