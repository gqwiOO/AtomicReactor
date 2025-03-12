using System;
using Gameplay.Buildings.Triggers;
using Gameplay.Map.Building;
using UnityEngine;

namespace Gameplay.Buildings.Listeners
{
    public class GameObjectSetActiveOnTriggered: MonoBehaviour
    {
        [SerializeField] private BaseBuildingMapObjectTrigger trigger;

        [SerializeField] private GameObject gameObject;

        private void Start()
        {
            trigger.OnTriggered += Trigger_OnTriggered;
            trigger.OnUntriggered += Trigger_OnUntriggered;
        }

        private void Trigger_OnUntriggered()
        {
            gameObject.SetActive(false);
        }

        private void Trigger_OnTriggered(BuildingMapObject obj)
        {
            gameObject.SetActive(true);
        }
    }
}