using System;
using System.Collections.Generic;
using _Project.Core.Services.UpdateService;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Mechanics.Raycast
{
    public class RaycastService : IRaycastService, IUpdatable
    {
        private List<RaycastResult> _raycastResults = new ();
        private IUpdateService _updateService;

        public IEnumerable<RaycastResult> Result => _raycastResults;
        
        public event Action<IEnumerable<RaycastResult>> OnSent;

        public UpdateType UpdateType => UpdateType.Update;
        
        [Inject]
        public RaycastService(IUpdateService updateService)
        {
            _updateService = updateService;
            _updateService.Add(this);
        }

        public void Tick()
        {
            if (EventSystem.current != null)
            {
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                EventSystem.current.RaycastAll(eventDataCurrentPosition, _raycastResults);
                OnSent?.Invoke(Result);
            }
        }
    }

    public interface IRaycastService
    {
        IEnumerable<RaycastResult> Result { get; }

        event Action<IEnumerable<RaycastResult>> OnSent;
    }
}