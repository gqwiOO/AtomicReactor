using System.Collections.Generic;
using Gameplay.Transportation.ItemPipeSystem;

namespace Gameplay.Transportation.Sorters
{
    public class ItemSorter: ItemPipe
    {
        private HashSet<int> _allowedItems = new ();
        
        
        public IEnumerable<int> AllowedItems => _allowedItems;
        
        public override void Tick()
        {
            
        }

        public void SetItemAllowedState(int itemId, bool allowed)
        {
            if (allowed)
            {
                _allowedItems.Add(itemId);
            }
            else
            {
                _allowedItems.Remove(itemId);
            }
        }

        protected override bool CanTransport(int itemId)
        {
            return _allowedItems.Contains(itemId);
        }
    }
}