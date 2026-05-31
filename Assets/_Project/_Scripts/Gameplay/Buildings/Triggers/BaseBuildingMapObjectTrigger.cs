using System;
using Gameplay.Map.Building;
using UnityEngine;

namespace Gameplay.Buildings.Triggers
{
    public class BaseBuildingMapObjectTrigger: MonoBehaviour
    {
        public event Action<BuildingMapObject> OnTriggered;
        public event Action OnUntriggered;
        
        public void Trigger(BuildingMapObject buildingMapObject)
        {
            OnTriggered?.Invoke(buildingMapObject);
        }

        public void Untrigger()
        {
            OnUntriggered?.Invoke();
        }
    }
}