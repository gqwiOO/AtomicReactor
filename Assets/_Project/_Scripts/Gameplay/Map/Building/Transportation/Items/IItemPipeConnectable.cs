namespace Gameplay.Transportation.ItemPipeSystem
{
    public interface IItemExtractionSource
    {
        bool HasItemsForPipe();
        int GetExtractableItemId();
        void ExtractForPipe(int itemId, int amount);
    }

    public interface IItemInsertionTarget
    {
        bool CanInsertFromPipe(int itemId, int amount = 1);
        void InsertFromPipe(int itemId, int amount);
    }
}
