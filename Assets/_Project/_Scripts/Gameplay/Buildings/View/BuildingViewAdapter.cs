using System.Collections.Generic;
using Gameplay.Map.Building;
using UnityEngine;

namespace Gameplay.Buildings.View
{
    public class BuildingViewAdapter: BaseBuildingView
    {
        [SerializeField] private List<BaseBuildingView> views;
        
        public override void Init(BuildingSettingsDataAsset buildingSettingsDataAsset)
        {
            views.ForEach(view => view.Init(buildingSettingsDataAsset));
            base.Init(buildingSettingsDataAsset);
        }

        public override void UpdateView()
        {
            views.ForEach(view => view.UpdateView());
        }
    }
}