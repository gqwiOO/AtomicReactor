using System;
using Gameplay.UI.Tabs;
using UnityEngine;

namespace Gameplay.Map.Control
{
    public class MapControlModeButton : TabButton
    {
        [SerializeField] private ControlMode _mode;

        public event Action<ControlMode> OnModeSelected;

        protected override void HandleClick()
        {
            OnModeSelected?.Invoke(_mode);            
        }
    }
}