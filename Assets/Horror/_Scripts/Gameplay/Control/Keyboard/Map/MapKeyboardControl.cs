using System.Collections.Generic;
using Core.Scripts.Debugging;
using Gameplay.Control.Data;
using Gameplay.Map.Cell;
using UnityEngine;
using Zenject;

namespace Gameplay.Control.Keyboard
{
    public class MapKeyboardControl : IMapKeyboardControl
    {
        private delegate void MapAction(ICell cell);

        private Dictionary<KeyCode, MapAction> _mapInteractionsDictionary = new Dictionary<KeyCode, MapAction>();
        private ICellMapListener _cellMapListener;


        [Inject]
        private void Construct(ICellMapListener cellMapListener)
        {
            _cellMapListener = cellMapListener;
        }

        public void Init(KeyboardSettings keyboardSettings)
        {
            _mapInteractionsDictionary.TryAdd(keyboardSettings.BuildingSidesSettingsKey, ConfigSidesInteraction);
        }

        private void ConfigSidesInteraction(ICell cell)
        {
            Debugging.Log(this,$"Config sides for cell : {cell.Position}");
        }

        public void HandleInteraction(KeyCode keyCode)
        {
            _mapInteractionsDictionary.TryGetValue(keyCode, out MapAction result);
            if (result != null)
                result.Invoke(_cellMapListener.CurrentCell);
        }
    }
}