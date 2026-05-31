using Gameplay.Transportation.WaterPipeSystem;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Views
{
    public class FloatContainerView: MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        
        private IFloatContainer _floatContainer;

        public void Init(IFloatContainer floatContainer)
        {
            if(_floatContainer != null)
                _floatContainer.OnValueChanged -= FloatContainer_OnValueChanged;
            
            _floatContainer = floatContainer;
            _floatContainer.OnValueChanged += FloatContainer_OnValueChanged;
            
            FloatContainer_OnValueChanged(_floatContainer.CurrentValue);
        }

        private void FloatContainer_OnValueChanged(float obj)
        {
            textField.text = obj.ToString("F");
        }
    }
}