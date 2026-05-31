using System;
using Gameplay.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Buildings.View
{
    public class BuildingItemView: BaseInteractableItem
    {
        [SerializeField] private BaseBuildingView buildingView;
    }
}