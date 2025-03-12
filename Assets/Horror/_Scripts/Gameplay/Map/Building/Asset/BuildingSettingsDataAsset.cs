using System;
using UnityEngine;

namespace Gameplay.Map.Building
{
    [CreateAssetMenu(menuName = "Core/Buildings/BuildingSettingsDataAsset", fileName = "BuildingSettingsDataAsset")]
    public abstract class BuildingSettingsDataAsset: ScriptableObject
    {
        [field: SerializeField] public BuildingMapObject BuildingMapObject { get; private set; }
        [field: SerializeField] public abstract string Key { get; }
        [field: SerializeField] public abstract string Name { get; }

        [field: SerializeField] public float Price { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}