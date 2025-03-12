using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building;
using Gameplay.Map.Building.View;
using Gameplay.MapUI.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.MapUI
{
    public class MapUIProvider: MonoBehaviour, IMapUIProvider
    {
        
        [SerializeField] 
        private ElectricFurnaceView electricFurnaceView;

        [SerializeField] 
        private List<BaseMapObjectView> _mapObjectsViews;

        private Dictionary<string, BaseMapObjectView> _mapObjectViewsDictionary;

        private void Awake()
        {
            _mapObjectViewsDictionary = _mapObjectsViews.ToDictionary(view => view.Key);
        }

        public ElectricFurnaceView GetElectricFurnaceView() => electricFurnaceView;
        
        public IMapUIObjectView GetMapUIView(string key)
        {
            _mapObjectViewsDictionary.TryGetValue(key, out BaseMapObjectView result);
            return result;
        }
    }
}