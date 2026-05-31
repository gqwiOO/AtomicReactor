using Gameplay.Map.Building;
using UnityEngine;

namespace Gameplay.Transportation.ItemPipeSystem
{
    [CreateAssetMenu(menuName = "Core/Buildings/ItemPipeSettingsDataAsset", fileName = "ItemPipeSettingsDataAsset")]
    public class ItemPipeSettingsDataAsset : BuildingSettingsDataAsset
    {
        [field: SerializeField] public float TransferInterval { get; private set; } = 0.5f;
        [field: SerializeField] public int TransferAmount { get; private set; } = 1;
    }
}
