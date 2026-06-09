using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay.Map.Control
{
    public class MapControlView: MonoBehaviour
    {
        [SerializeField] private List<MapControlModeButton> modesButtons;
        private MapControlService _mapControlService;

        [Inject]
        private void Construct(MapControlService mapControlService)
        {
            _mapControlService = mapControlService;
        }
        
        private void Start()
        {
            foreach (MapControlModeButton modeButton in modesButtons)
            {
                modeButton.OnModeSelected += ModeButton_OnModeSelected;
            }
        }

        private void OnDestroy()
        {
            foreach (MapControlModeButton modeButton in modesButtons)
            {
                modeButton.OnModeSelected += ModeButton_OnModeSelected;
            }
        }

        private void ModeButton_OnModeSelected(ControlMode controlMode)
        {
            _mapControlService.SelectControlMode(controlMode);   
        }
    }
}