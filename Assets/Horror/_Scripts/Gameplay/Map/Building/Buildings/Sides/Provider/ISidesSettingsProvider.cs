using NUnit.Framework;
using UnityEngine;

namespace Gameplay.Map.Building.Provider
{
    public interface ISidesSettingsProvider
    {
        Sprite GetSideSprite(SideType sideType);
    }
}