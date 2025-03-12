using System;
using Gameplay.MapUI.Views;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.Map.Building.View
{
    public class BuildingSidesView: BaseMapObjectView
    {
        [SerializeField]
        private SideView leftView;
        [SerializeField] 
        private SideView rightView;
        [SerializeField] 
        private SideView frontView;
        [SerializeField] 
        private SideView backView;
        
        private BuildingSidesData _buildingSidesData;

        private void SpecificInit(BuildingSidesData buildingSidesData)
        {
            _buildingSidesData = buildingSidesData;
            
            InitSidesViews();

            leftView.OnSideSettingsChanged += LeftView_OnSideSettingsChanged;
            rightView.OnSideSettingsChanged += RightView_OnSideSettingsChanged;
            frontView.OnSideSettingsChanged += FrontView_OnSideSettingsChanged;
            backView.OnSideSettingsChanged += BackView_OnSideSettingsChanged;
        }

        private void InitSidesViews()
        {
            leftView.Init(_buildingSidesData.Left);
            rightView.Init(_buildingSidesData.Right);
            frontView.Init(_buildingSidesData.Front);
            backView.Init(_buildingSidesData.Back);
        }
        
        private void LeftView_OnSideSettingsChanged(SideType previousSideType)
        {
            var sideType = NextSideType(previousSideType);
            _buildingSidesData.Left = sideType;
            InitSidesViews();
        }

        private void RightView_OnSideSettingsChanged(SideType obj)
        {
            var sideType = NextSideType(obj);
            _buildingSidesData.Right = sideType;
            InitSidesViews();
        }

        private void FrontView_OnSideSettingsChanged(SideType obj)
        {
            var sideType = NextSideType(obj);
            _buildingSidesData.Front = sideType;
            InitSidesViews();
        }

        private void BackView_OnSideSettingsChanged(SideType obj)
        {
            var sideType = NextSideType(obj);
            _buildingSidesData.Back = sideType;
            InitSidesViews();
        }

        private static SideType NextSideType(SideType obj)
        {
            int maxValue = Enum.GetValues(typeof(SideType)).Length - 1;
            int newValue = (int)obj + 1;
            var sideType = (SideType)(newValue > maxValue ? 1 : newValue);
            return sideType;
        }

        public override void Init(BuildingMapObject buildingMapObject)
        {
            var sidesBuildingMapObject = buildingMapObject as ISidesBuildingMapObject;
            SpecificInit(sidesBuildingMapObject?.BuildingSidesData);
        }
        
        
    }
}