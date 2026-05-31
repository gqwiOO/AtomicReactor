using UnityEngine;

namespace Gameplay.Control.Data
{
    [CreateAssetMenu(menuName = "Core/Control/KeyboardSettingsAsset", fileName = "KeyboardSettingsAsset")]
    public class KeyboardSettingsAsset: ScriptableObject
    {
        [field: SerializeField] public KeyboardSettings KeyboardSettings { get; private set; }
    }
}