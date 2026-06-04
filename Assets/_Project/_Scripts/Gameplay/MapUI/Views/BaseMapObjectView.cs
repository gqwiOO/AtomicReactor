using System.Collections.Generic;
using Gameplay.Map.Building;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.MapUI.Views
{
    public abstract class BaseMapObjectView: MonoBehaviour, IMapUIObjectView
    {
        [field: SerializeField]
        public List<string> Keys { get; private set;}

        [SerializeField] private bool hasCloseButton;
        
        [ShowIf(nameof(hasCloseButton))]
        [SerializeField] private Button closeButton;
        
        public abstract void Init(BuildingMapObject buildingMapObject);


        protected virtual void Start()
        {
            if (hasCloseButton)
                closeButton.onClick.AddListener(Hide);
        }


        protected virtual void OnDestroy()
        {
            if (hasCloseButton)
                closeButton.onClick.RemoveListener(Hide);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

    public class WirePoleView : BaseMapObjectView
    {
        [SerializeField] private TMP_Text inputPowerText;
        [SerializeField] private TMP_Text outputPowerText;
        
        public override void Init(BuildingMapObject buildingMapObject)
        {
            
        }

        private void SpecifiInit()
        {
            
        }
    }

    public interface IMapUIObjectView
    {
        void Show();
        void Hide();
        void Init(BuildingMapObject buildingMapObject);
    }
}