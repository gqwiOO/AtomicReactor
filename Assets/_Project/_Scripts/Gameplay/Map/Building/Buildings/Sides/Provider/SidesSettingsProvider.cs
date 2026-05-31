using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Map.Building.Provider
{
    public class SidesSettingsProvider : MonoBehaviour,ISidesSettingsProvider
    {
        [Serializable]
        class SideSpriteSettings
        {
            public SideType SideType;
            public Sprite Sprite;
        }
        
        [SerializeField] private List<SideSpriteSettings> sideSpriteSettingsList;
        
        
        public Sprite GetSideSprite(SideType sideType)
        {
            return sideSpriteSettingsList.FirstOrDefault(s => s.SideType == sideType)?.Sprite;
        }
    }
}