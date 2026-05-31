using UnityEngine;

namespace Gameplay.Control
{
    public class CameraMapDrag: MonoBehaviour
    {
        [SerializeField] private float dragSpeed = 1.0f;
        [SerializeField] private float zoomSpeed = 5.0f;
        [SerializeField] private float minZoom = 3.0f;
        [SerializeField] private float maxZoom = 40.0f;

        private Vector3 lastMousePosition;

        private void Update()
        {
            HandleDrag();
            HandleZoom();
        }

        private void HandleDrag()
        {
            if (Input.GetMouseButtonDown(1))
                lastMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(1))
            {
                float zoomFactor = GetZoomFactor();
                Vector3 delta = Input.mousePosition - lastMousePosition;
                transform.position -= new Vector3(delta.x, 0, delta.y) * (dragSpeed * zoomFactor * Time.deltaTime);
                lastMousePosition = Input.mousePosition;
            }
        }

        private float GetZoomFactor()
        {
            var cam = Camera.main;
            if (cam == null) return 1f;

            float zoom = cam.orthographic ? cam.orthographicSize : transform.position.y;
            return zoom / minZoom;
        }

        private void HandleZoom()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (scroll == 0f) return;

            var cam = Camera.main;
            if (cam == null) return;

            if (cam.orthographic)
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                Vector3 pos = transform.position;
                pos.y = Mathf.Clamp(pos.y - scroll * zoomSpeed, minZoom, maxZoom);
                transform.position = pos;
            }
        }
    }
}