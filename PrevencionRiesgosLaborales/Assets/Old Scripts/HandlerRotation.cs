using System;
using UnityEngine;

public class HandlerRotation : HandlersTransfrom
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

        float rotationAmount = CalculateRotationAmount(deltaMouse);
    
        // Aplicar rotación en torno al eje "forward" del objeto
        _transform.Rotate(_forwardDirection, rotationAmount * sensitivity * deltaMouse.magnitude, Space.World);
    }

    private float CalculateRotationAmount(Vector3 deltaMouse)
    {
        // Proyectamos los vectores al plano XZ
        Vector3 deltaMouseXZ = new Vector3(deltaMouse.x, 0f, deltaMouse.y);
        Vector3 forwardDirectionXZ = new Vector3(_forwardDirection.x, 0f, _forwardDirection.z);
    
        // Calculamos el ángulo entre deltaMouse y la dirección forward
        float angle = Vector3.Angle(deltaMouseXZ, forwardDirectionXZ);

        // Retornar un valor proporcional al ángulo (puedes ajustar este valor según tus necesidades)
        return angle;
    }

}