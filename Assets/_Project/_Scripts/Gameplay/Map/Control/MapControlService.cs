using System.Collections.Generic;
using Gameplay.Control;
using Gameplay.Map.Building;
using Gameplay.Map.Cell;
using UnityEngine.Windows;
using Zenject;
using Input = UnityEngine.Input;

namespace Gameplay.Map.Control
{

    public class ControlArgs
    {
        public string BuildingKey { get; set; }
        public BuildingMapObject BuildingMapObject { get; set; }
    }
    public class MapControlService: ITickable
    {
        private Dictionary<ControlMode, IMapControlTypeHandler> _controlTypeHandlers;
        
        private ControlMode _activeControlMode;
        private IMapControlTypeHandler _activeControlHandler;

        
        private ICellMapListener _cellMapListener;

        [Inject]
        public MapControlService(List<IMapControlTypeHandler> controlTypeHandlers, ICellMapListener cellMapListener)
        {
            _cellMapListener = cellMapListener;
            _controlTypeHandlers = new Dictionary<ControlMode, IMapControlTypeHandler>();
            
            foreach (IMapControlTypeHandler typeHandler in controlTypeHandlers)
            {
                _controlTypeHandlers.Add(typeHandler.ControlMode, typeHandler);
                typeHandler.OnSelfChangeModeToDefault += () => SelectControlMode(ControlMode.Default);
            }
            
            
            _cellMapListener.OnCellPointed += CellMapListener_OnCellPointed;
            
            SelectControlMode(ControlMode.Default);
        }

        private void CellMapListener_OnCellPointed(CellComponent cell)
        {
            _activeControlHandler?.HandleCellChanged(cell);
        }

        public void SetBuildingControlMode(string buildingKey)
        {
            ControlArgs controlArgs = new ControlArgs()
            {
                BuildingKey = buildingKey
            };
            UnselectControlMode();
            SelectControlMode(ControlMode.Building, controlArgs);
        }
        
        public void SelectControlMode(ControlMode mode, ControlArgs controlArgs = null)
        {
            _activeControlMode = mode;
            _activeControlHandler = GetControlHandler(mode);
            _activeControlHandler.Init(controlArgs);
            _activeControlHandler.SetState(true);
        }

        private void UnselectControlMode()
        {
            _activeControlHandler?.SetState(false);
        }

        private IMapControlTypeHandler GetControlHandler(ControlMode mode)
        {
            return _controlTypeHandlers[mode];       
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
                _activeControlHandler?.HandleCellClick(_cellMapListener.CurrentCell);

            if (Input.GetMouseButtonUp(0))
                _activeControlHandler?.HandleCellRelease(_cellMapListener.CurrentCell);
        }
    }
}