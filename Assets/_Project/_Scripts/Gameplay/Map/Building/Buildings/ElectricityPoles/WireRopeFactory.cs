using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

namespace Gameplay.Map.Building.ElectricityPoles
{
    public class WireRopeFactory: MonoBehaviour
    {
        [SerializeField] private Rope ropePrefab;

        [SerializeField] private float  distanceMultiplier;
        
        private static WireRopeFactory _instance;

        public static WireRopeFactory Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        public void SpawnRope(Transform point1, Transform point2)
        {
            var rope = Instantiate(ropePrefab).GetComponent<Rope>();
            rope.InitializeLineRenderer();
            rope.SetStartPoint(point1);
            rope.SetEndPoint(point2);
            rope.ropeLength = Vector3.Distance(point1.position, point2.position) * distanceMultiplier;
            rope.RecalculateRope();
        }
    }
}