using System;
using UnityEngine;

public class ArrowMovement : HandlersTransfrom
{

    public void OnMouseDown()
    {
       base.OnMouseDown();
    }

    public void OnMouseDrag()
    {
       base.OnMouseDrag();
    }

    public void OnMouseUp()
    {
       base.OnMouseUp();
    }

    protected override void ApplyTransform()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 deltaMouse = _lastMousePosition - currentMousePosition;
        _lastMousePosition = currentMousePosition;

        float movementDirection = CalculateMovementDirection(deltaMouse);
            
        _transform.Translate(movementDirection * _forwardDirection * sensitivity * deltaMouse.magnitude, Space.World);
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