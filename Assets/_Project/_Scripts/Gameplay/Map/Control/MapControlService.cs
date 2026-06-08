namespace Gameplay.Map.Control
{
    public class MapControlService
    {
        private ControlMode _currentControlMode;
        
        public void SelectControlMode(ControlMode mode)
        {
            _currentControlMode = mode;
        }
    }
}