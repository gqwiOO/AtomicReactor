using UnityEngine;

namespace Audio.Music
{
    [CreateAssetMenu(menuName = "GameAssets/Audio/Music/MusicConfig", fileName = "MusicConfig", order = 0)]
    public class MusicDataAsset: ScriptableObject
    {
        [field: SerializeField] public MusicData MusicData { get; private set; }
    }
}