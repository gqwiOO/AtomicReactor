using UnityEngine;

namespace Gameplay.Control
{
    public class CameraMapDrag: MonoBehaviour
    {
        [SerializeField] private float dragSpeed = 1.0f;
        private Vector3 lastMousePosition;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                lastMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(1))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                transform.position -= new Vector3(delta.x, 0, delta.y) * (dragSpeed * Time.deltaTime);
                lastMousePosition = Input.mousePosition;
            }
        }
    }    
}