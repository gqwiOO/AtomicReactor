using System.Collections.Generic;
using Zenject;

namespace Gameplay.Map.Control
{
    public class MapControlService
    {
        private Dictionary<ControlMode, IMapControlTypeHandler> _controlTypeHandlers;
        
        private ControlMode _activeControlMode;
        private IMapControlTypeHandler _activeControlHandler;

        [Inject]
        public MapControlService(List<IMapControlTypeHandler> controlTypeHandlers)
        {
            _controlTypeHandlers = new Dictionary<ControlMode, IMapControlTypeHandler>();
            foreach (IMapControlTypeHandler typeHandler in controlTypeHandlers)
            {
                _controlTypeHandlers.Add(typeHandler.ControlMode, typeHandler);
            }
        }
        
        public void SelectControlMode(ControlMode mode)
        {
            _activeControlMode = mode;
            _activeControlHandler = GetControlHandler(mode);
        }

        private IMapControlTypeHandler GetControlHandler(ControlMode mode)
        {
            return _controlTypeHandlers[mode];       
        }
    }
}