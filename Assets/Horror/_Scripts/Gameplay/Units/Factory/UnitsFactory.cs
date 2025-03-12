using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.Factory
{
    public class UnitsFactory : MonoBehaviour, IUnitsFactory
    {
        [SerializeField] private List<BaseBarrackBarackUnit> unitsList;
        private DiContainer _diContainer;
        
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public IBarackUnit SpawnUnit(string unitKey, Vector3 position)
        {
            BaseBarrackBarackUnit prefab = unitsList.FirstOrDefault(u => u.Key == unitKey);
            BaseBarrackBarackUnit instance = _diContainer.InstantiatePrefabForComponent<BaseBarrackBarackUnit>(prefab, position, quaternion.identity, null);
            return instance;
        }
    }
}