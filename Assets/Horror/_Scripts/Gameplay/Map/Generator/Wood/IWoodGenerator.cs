using System;
using Cysharp.Threading.Tasks;

namespace Gameplay.Map.Generating.Wood
{
    public interface IWoodGenerator
    {
        UniTask GenerateWoods();
    }
}