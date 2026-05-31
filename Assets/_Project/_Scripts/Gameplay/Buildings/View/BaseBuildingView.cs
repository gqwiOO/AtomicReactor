using System;
using Gameplay.Map.Building;
using Gameplay.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay.Buildings.View
{
    public abstract class BaseBuildingView: MonoBehaviour
    {
        [SerializeField] private bool hasInteractableItem;
        
        [field: ShowIf(nameof(hasInteractableItem))] 
        [field: SerializeField]
        public BaseInteractableItem InteractableItem { get; protected set; }
        
        protected BuildingSettingsDataAsset _buildingSettingsDataAsset;

        public string BuildingKey => _buildingSettingsDataAsset.Key;
        
        public virtual void Init(BuildingSettingsDataAsset buildingSettingsDataAsset)
        {
            _buildingSettingsDataAsset = buildingSettingsDataAsset;
            UpdateView();
            if(hasInteractableItem)
                InteractableItem.Init(_buildingSettingsDataAsset.Key);
        }

        public abstract void UpdateView();
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}