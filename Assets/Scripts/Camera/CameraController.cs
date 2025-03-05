using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    private Vector3 lastMousePosition;
    
    void Update()
    {
        if (Application.isEditor)
        {
            HandleEditorControls();
        }
        else
        {
            HandleTouchControls();
        }
    }

    void HandleEditorControls()
    {
        if (Input.GetMouseButton(1)) 
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            transform.position += transform.forward * scroll * zoomSpeed;
            ClampZoom();
        }

        lastMousePosition = Input.mousePosition;
    }

    void HandleTouchControls()
    {
        if (Input.touchCount == 2) 
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                Vector2 deltaTouch = (touch0.deltaPosition + touch1.deltaPosition) / 2;
                Vector3 move = new Vector3(-deltaTouch.x, 0, -deltaTouch.y) * moveSpeed * Time.deltaTime;
                transform.Translate(move, Space.World);
            }
            float prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
            float currentDistance = (touch0.position - touch1.position).magnitude;
            float zoomDelta = (prevDistance - currentDistance) * zoomSpeed * 0.01f;

            transform.position += transform.forward * zoomDelta;
            ClampZoom();
        }
    }

    void ClampZoom()
    {
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        if (distance < minZoom)
            transform.position = transform.position.normalized * minZoom;
        else if (distance > maxZoom)
            transform.position = transform.position.normalized * maxZoom;
    }
}
