using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Buildings.View
{
    public class BuildingIconView : BaseBuildingView
    {
        [SerializeField] private Image image;
        
        public override void UpdateView()
        {
            image.sprite = _buildingSettingsDataAsset.Icon;
        }
    }
}