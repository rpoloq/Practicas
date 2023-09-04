using UnityEngine;
using System;
using Unity.VisualScripting;

public class ObjectSelection : MonoBehaviour
{
    public LayerMask selectableLayer; // Capa de los objetos seleccionables
    public event Action OnDeselectObject;
    
    private Outline _outline;
    private bool _isSelected = false;
    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }
    void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer))
            {
                GameObject newSelectedObject = hit.collider.gameObject;
                
                // Si se selecciona un nuevo objeto se desactiva el outline 
                if (newSelectedObject != gameObject && _isSelected)
                {
                    UnhighlightObject();
                }
                
                // El objeto seleccionado se destaca y si ya estaba seleccionado se desactiva el outline
                if (newSelectedObject == gameObject)
                {
                    if (!_isSelected)
                    {
                        HighlightObject();
                    }
                    else
                    {
                        UnhighlightObject();
                    }
                } 
            } else if (_isSelected)
            {
                // Si se clica fuera o en un objeto no seleccionable se quita el outline
                UnhighlightObject();
                OnDeselectObject?.Invoke();
            }
        }
    }

    private void HighlightObject()
    {
        _isSelected = true;
        _outline.enabled = true;
    }

    private void UnhighlightObject()
    {
        _isSelected = false;
        _outline.enabled = false;
    }
}