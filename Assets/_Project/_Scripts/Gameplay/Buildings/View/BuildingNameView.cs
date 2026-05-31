using System;
using TMPro;
using UnityEngine;

namespace Gameplay.Buildings.View
{
    public class BuildingNameView : BaseBuildingView
    {
        [SerializeField] private TMP_Text textField;
        
        private void OnValidate()
        {
            textField ??= GetComponent<TMP_Text>();
        }

        public override void UpdateView()
        {
            textField.text = _buildingSettingsDataAsset.Name;
        }
    }
}