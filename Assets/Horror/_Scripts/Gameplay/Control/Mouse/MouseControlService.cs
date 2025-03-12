using System.Collections.Generic;
using System.Linq;
using Mechanics.Raycast;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Control.Mouse
{
    public class MouseControlService : MonoBehaviour,IMouseControlService
    {
        [field: SerializeField] 
        public LayerMask uiLayerMask { get; private set; }
        
        private IRaycastService _raycastService;
        public bool IsMouseOverUI { get; private set; }

        [Inject]
        private void Construct(IRaycastService raycastService)
        {
            _raycastService = raycastService;
        }

        private void Awake() => _raycastService.OnSent += RaycastService_OnSent;

        private void OnDestroy() => _raycastService.OnSent -= RaycastService_OnSent;

        private void RaycastService_OnSent(IEnumerable<RaycastResult> obj)
        {
            IsMouseOverUI = obj.Any();
        }
    }
}