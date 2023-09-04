using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHandlesManager : MonoBehaviour
{
    private ObjectSelection _seleccionObjeto;
    private Transform _movementArrows;
    private Transform _rotationHandlers;
    
    private void OnEnable()
    {
        // Obtener una referencia al ObjectSelection
        ObjectSelection objectSelection = GetComponent<ObjectSelection>();

        // Suscribirse al evento
        if (objectSelection != null)
        {
            objectSelection.OnDeselectObject += DisableTransformHandlers;
        }
    }
    
    private void OnDisbale()
    {
        // Obtener una referencia al ObjectSelection
        ObjectSelection objectSelection = GetComponent<ObjectSelection>();

        // Desuscribirse al evento
        if (objectSelection != null)
        {
            objectSelection.OnDeselectObject -= DisableTransformHandlers;
        }
    }
    void Awake()
    {
        _seleccionObjeto = GetComponent<ObjectSelection>();
        
        Transform localTransform = transform; 
        _movementArrows = localTransform.GetChild(1).transform;
        _rotationHandlers = localTransform.GetChild(2).transform;
        // float scaleFactor = localTransform.lossyScale.magnitude;
        // _movementArrows.localScale = Vector3.one * scaleFactor;
        // _rotationHandlers.localScale = Vector3.one * scaleFactor;
    }

    void Update()
    {
        if (!_seleccionObjeto.IsSelected)
            return;
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            _movementArrows.gameObject.SetActive(true);
            _rotationHandlers.gameObject.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            _movementArrows.gameObject.SetActive(false);
            _rotationHandlers.gameObject.SetActive(true);
        }
    }

    private void DisableTransformHandlers()
    {
        _movementArrows.gameObject.SetActive(false);
        _rotationHandlers.gameObject.SetActive(false);
    }
}
