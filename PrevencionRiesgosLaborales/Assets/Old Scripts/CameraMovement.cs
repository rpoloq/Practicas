using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 0.5f; // Sensibilidad del movimiento de la cámara
    public float zoomSpeed = 5f; // Velocidad de zoom de la cámara
    public float dragSpeed = 2f; // Velocidad de desplazamiento en el plano XY

    private Vector3 lastMousePosition;
    private Transform _transform;
    private bool _isDragging = false;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        // Movimiento de la cámara al pulsar el botón derecho del ratón
        MoverCamara();

        // Zoom de la cámara utilizando la rueda del ratón
        ZoomCamara();
        
        // Desplazamiento de la cámara en el plano XY manteniendo pulsada la rueda del ratón
        DesplazarCamara();
    }

    private void MoverCamara()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
            Vector3 rotation = new Vector3(-deltaMouse.y, deltaMouse.x, 0) * sensitivity;
            _transform.eulerAngles += rotation;
            lastMousePosition = Input.mousePosition;
        }
    }

    private void ZoomCamara()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput != 0)
        {
            float zoomAmount = scrollWheelInput * zoomSpeed;
            Vector3 zoom = transform.forward * zoomAmount;
            transform.Translate(zoom, Space.World);
        }
    }

    private void DesplazarCamara()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(2))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            float deltaX = -Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;
            float deltaY = -Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
            Vector3 drag = new Vector3(deltaX, deltaY, 0);
            transform.Translate(drag, Space.Self);
        }
    }
}

