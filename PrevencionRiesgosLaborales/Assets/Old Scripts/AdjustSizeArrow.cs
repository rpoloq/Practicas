using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSizeArrow : MonoBehaviour
{
    public float fixedSize = 1.0f; // Tamaño constante deseado de las flechas
    private Camera _mainCamera; // Referencia a la cámara principal

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // Calcular la distancia desde la cámara al objeto
        float distanceToCamera = Vector3.Distance(_mainCamera.transform.position, transform.position);

        // Calcular el tamaño en función de la distancia y el campo de visión de la cámara
        float scaleFactor = fixedSize / (Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * distanceToCamera * 2.0f);

        // Ajustar el tamaño del objeto en función del scaleFact
    }
}
