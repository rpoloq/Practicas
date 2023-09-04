using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandlersTransfrom : MonoBehaviour
{
    public float sensitivity = 0.01f;
    public Material hightlightMaterial;
    
    protected bool _isDragging = false;
    protected Vector3 _lastMousePosition;
    protected Vector3 _forwardDirection;
    protected Material _originalMaterial;
    protected Transform _transform;
    protected Renderer _renderer;

    private void Awake()
    {
        _transform = transform.parent.transform.parent.transform;
        _renderer = GetComponentInChildren<Renderer>();
        _originalMaterial = _renderer.material;
    }

    void Start()
    {
        _forwardDirection = transform.forward;
        _forwardDirection.Normalize();
    }

    protected abstract void ApplyTransform();

    public void OnMouseDown()
    {
        _isDragging = true;
        _lastMousePosition = Input.mousePosition;
        _renderer.material = hightlightMaterial;
    }

    public void OnMouseDrag()
    {
        if (_isDragging)
        {
            ApplyTransform();
        }
    }

    public void OnMouseUp()
    {
        _isDragging = false;
        _renderer.material = _originalMaterial;
    }

    private float CalculateMovementDirection(Vector3 deltaMouse)
    {
        // Proyectamos los vectores al plano XZ
        Vector3 deltaMouseXZ = new Vector3(deltaMouse.x, 0f, deltaMouse.y);
        Vector3 forwardDirectionXZ = new Vector3(_forwardDirection.x, 0f, _forwardDirection.z);
        
        // Calculamos el ángulo entre deltaMouse y la dirección forward
        float angle = Vector3.Angle(deltaMouseXZ, forwardDirectionXZ);

        // Si el ángulo es mayor a 90 grados, nos movemos hacia atrás, en caso contrario, hacia adelante
        return angle > 90f ? -1f : 1f;
    }
}
