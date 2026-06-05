using System;
using System.Collections.Generic;
using Gameplay.Map.Building.Items.Provider;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Map.Building.Items.View
{
    public class ItemView: MonoBehaviour
    {
        [SerializeField] private ItemIconView iconView;

        [SerializeField] private bool isInteractable;
        [SerializeField] private bool selectable;
        
        [ShowIf(nameof(isInteractable))]
        [SerializeField] private Button button;
        
        [ShowIf(nameof(selectable))] 
        [SerializeField]
        private List<GameObject> selectedState;
        
        [ShowIf(nameof(selectable))] 
        [SerializeField]
        private List<GameObject> unselectedState;
        
        public bool IsSelected { get; private set; }
        
        private IItemsDataProvider _itemsDataProvider;
        private int _itemId;

        public event Action<ItemView,int> OnClicked;

        [Inject]
        private void Construct(IItemsDataProvider itemsDataProvider)
        {
            _itemsDataProvider = itemsDataProvider;
        }

        private void Start()
        {
            if(isInteractable)
                button.onClick.AddListener(Button_OnClick);
        }

        private void OnDestroy()
        {
            if(isInteractable)
                button.onClick.RemoveListener(Button_OnClick);
        }

        private void Button_OnClick() => OnClicked?.Invoke(this, _itemId);

        public void Set(int itemId)
        {
            _itemId = itemId;
            iconView.Set(itemId);
        }
        
        public void SetAvailable(bool isAvailable)
        {
            iconView.SetAvailableState(isAvailable);
        }
        
        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            foreach (var state in selectedState)
                state.SetActive(isSelected);
            foreach (var state in unselectedState)
                state.SetActive(!isSelected);
        }
    }
}