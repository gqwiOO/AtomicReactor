using System;
using System.Collections.Generic;
using Gameplay.Map.Building;
using Mechanics.Pools;
using UnityEngine;

namespace Gameplay.Buildings.View
{
    public class BuildingsCollectionView: MonoBehaviour
    {
        [SerializeField] private BuildingSettingsDataAssetsCollection buildingSettingsDataAssetsCollection;
        
        [SerializeField] private PoolGameObjects buildingViewsPool;

        [SerializeField] private Transform container;
        
        private Dictionary<string,BaseBuildingView> buildingViews = new ();
        
        public IEnumerable<BaseBuildingView> BuildingViews => buildingViews.Values;
        
        public event Action<string> OnBuildingToBuildSelected; 

        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            buildingViewsPool.Initialize();
            Init(buildingSettingsDataAssetsCollection.Collection);
        }
        
        protected void Init(IEnumerable<BuildingSettingsDataAsset> buildingSettingsDataAssets)
        {
            foreach (BuildingSettingsDataAsset dataAsset in buildingSettingsDataAssets)
            {
                if (buildingViews.ContainsKey(dataAsset.Key))
                {
                    buildingViews.TryGetValue(dataAsset.Key, out BaseBuildingView buildingView);
                    buildingView.Init(dataAsset);
                    buildingView.InteractableItem.OnClicked += OnBuildingToBuildSelected;
                }
                else
                {
                    BaseBuildingView buildingView = GetBuildingView();
                    buildingView.transform.SetParent(container);
                    buildingView.Init(dataAsset);
                    buildingView.Show();
                    buildingViews.Add(dataAsset.Key, buildingView);
                    buildingView.InteractableItem.OnClicked += OnBuildingToBuildSelected;
                }
            }
        }

        private BaseBuildingView GetBuildingView()
        {
            BaseBuildingView result = buildingViewsPool.Pull().GetOwner<BuildingViewAdapter>();
            return result;
        }
    }
}