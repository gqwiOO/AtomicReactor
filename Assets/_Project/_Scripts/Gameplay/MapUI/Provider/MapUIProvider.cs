using System.Collections.Generic;
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
            _mapObjectViewsDictionary = new Dictionary<string, BaseMapObjectView>();
            foreach (var view in _mapObjectsViews)
                foreach (var key in view.Keys)
                    _mapObjectViewsDictionary[key] = view;
        }

        public ElectricFurnaceView GetElectricFurnaceView() => electricFurnaceView;
        
        public IMapUIObjectView GetMapUIView(string key)
        {
            _mapObjectViewsDictionary.TryGetValue(key, out BaseMapObjectView result);
            return result;
        }
    }
}