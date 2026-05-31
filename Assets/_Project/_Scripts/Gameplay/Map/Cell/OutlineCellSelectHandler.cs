using UnityEngine;

namespace Gameplay.Map.Cell
{
    public class OutlineCellSelectHandler : CellSelectHandler
    {
        [SerializeField] 
        private GameObject outLine;
        
        public override void SelectCell() => outLine.gameObject.SetActive(true);

        public override void UnselectCell() => outLine.gameObject.SetActive(false);
    }
}