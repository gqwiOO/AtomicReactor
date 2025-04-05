using Gameplay.MapUI.Views;
using TMPro;
using UnityEngine;

namespace Gameplay.Map.Building.Generators.View
{
    public class WindGeneratorView: BaseMapObjectView
    {
        [SerializeField] private TMP_Text textField;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            SpecificInit(buildingMapObject as WindGeneratorMapObject); 
        }

        private void SpecificInit(WindGeneratorMapObject buildingMapObject)
        {
            textField.text = $"{buildingMapObject.Power} W";
        }
    }
}