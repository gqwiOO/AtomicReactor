using System;
using System.Linq;
using Gameplay.Map.Creator;
using UnityEngine;

namespace Gameplay.Map.Debug
{
    public class MapCheatPanel : MonoBehaviour
    {
        [SerializeField] private MapCreator _mapCreator;

        private bool _visible;
        private int _selectedIndex;
        private string[] _biomeNames;
        private BiomeType[] _biomeValues;

        private void Awake()
        {
            _biomeValues = Enum.GetValues(typeof(BiomeType))
                .Cast<BiomeType>()
                .Where(b => b != BiomeType.None)
                .ToArray();

            _biomeNames = _biomeValues.Select(b => b.ToString()).ToArray();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
                _visible = !_visible;
        }

        private void OnGUI()
        {
            if (!_visible)
                return;

            GUILayout.BeginArea(new Rect(10, 10, 220, 300));
            GUILayout.Box("~ Cheat Panel");

            GUILayout.Label("Biome:");
            _selectedIndex = GUILayout.SelectionGrid(_selectedIndex, _biomeNames, 1);

            GUILayout.Space(8);

            if (GUILayout.Button("Apply"))
                _mapCreator.RegenerateWithForcedBiome(_biomeValues[_selectedIndex]);

            GUILayout.EndArea();
        }
    }
}
