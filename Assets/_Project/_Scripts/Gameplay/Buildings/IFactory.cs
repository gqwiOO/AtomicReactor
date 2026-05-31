using System;

namespace Gameplay.Buildings
{
    public interface IFactory<TFactoryProduct> where TFactoryProduct : IFactoryProduct
    {
        event Action OnProducedItem;
    }

    public interface IFactoryProduct: IResource
    {
        
    }

    public interface IResource
    {
        int Key { get; }
        string Name { get; }
    }
}