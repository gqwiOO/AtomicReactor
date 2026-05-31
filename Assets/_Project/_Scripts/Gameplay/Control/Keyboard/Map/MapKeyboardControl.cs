using Core.Scripts.Debugging;
using Gameplay.Control.Data;
using Gameplay.Map.Cell;
using Zenject;

namespace Gameplay.Control.Keyboard
{
    public class MapKeyboardControl : IMapKeyboardControl
    {
        private ICellMapListener _cellMapListener;
        private IKeyboardManager _keyboardManager;

        [Inject]
        private void Construct(ICellMapListener cellMapListener, IKeyboardManager keyboardManager)
        {
            _cellMapListener = cellMapListener;
            _keyboardManager = keyboardManager;
        }

        public void Init(KeyboardSettings keyboardSettings)
        {
            _keyboardManager.RegisterAction(GameAction.ConfigBuildingSides, OnConfigSides);
        }

        private void OnConfigSides()
        {
            if (_cellMapListener.CurrentCell == null) return;
            Debugging.Log(this, $"Config sides for cell: {_cellMapListener.CurrentCell.Position}");
        }
    }
}
