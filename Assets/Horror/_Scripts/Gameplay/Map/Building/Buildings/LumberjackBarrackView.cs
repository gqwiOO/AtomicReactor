using Gameplay.Inventories.Views;
using Gameplay.MapUI.Views;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Map.Building
{
    public class LumberjackBarrackView: BaseMapObjectView
    {
        [SerializeField] private InventoryCellView inventoryCellView;
        
        public override void Init(BuildingMapObject buildingMapObject) 
            => SpecificInit(buildingMapObject as LumberjackBarrack);

        private void SpecificInit(LumberjackBarrack lumberjackBarrack) 
            => inventoryCellView.Init(lumberjackBarrack.InventoryCell);
    }
}