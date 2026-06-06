using Gameplay.Map.Building.Fluids.Tanks;
using Gameplay.Transportation.WaterPipeSystem;
using Gameplay.UI.Views;
using UnityEngine;

namespace Gameplay.Map.Building.Generators.Steam
{
    public class SteamGeneratorView : BaseGeneratorView
    {
        [SerializeField] private FloatContainerView steamView;
        
        public override void Init(BuildingMapObject buildingMapObject)
            => SpecificInit(buildingMapObject as SteamElectricityGeneratorMapObject);
        private void SpecificInit(SteamElectricityGeneratorMapObject steamElectricityGeneratorMapObject)
        {
            steamView.Init(steamElectricityGeneratorMapObject.SteamContainer.FloatContainer);
            
            InitElectricityView(steamElectricityGeneratorMapObject.ElectricityProvider.ElectricityContainer);
        }

    }
}