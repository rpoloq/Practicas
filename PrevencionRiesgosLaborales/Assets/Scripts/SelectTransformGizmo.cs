using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RuntimeHandle;

public class SelectTransformGizmo : MonoBehaviour
{
    // Variables públicas para los materiales
    public Material _highlightMaterial;
    public Material _selectionMaterial;

    // Variables privadas para materiales originales, objetos y configuración
    private Material _originalMaterial_highlight;
    private Material _originalMaterial_selection;
    private Transform _highlight;
    private Transform _selection;
    private RaycastHit _raycastHit;
    private RaycastHit _raycastHitHandle;
    private GameObject _runtimeTransformGameObj;
    private RuntimeTransformHandle _runtimeTransformHandle;
    private int _runtimeTransformLayer = 6;
    private int _runtimeTransformLayerMask;

    private void Start()
    {
        InitializeRuntimeTransformObjects();
    }

    void Update()
    {
        HandleHighlight();
        HandleSelection();
        HandleHotKeysForTransform();
    }

    // Inicializa los objetos para el manejo de transformación en tiempo de ejecución
    private void InitializeRuntimeTransformObjects()
    {
        _runtimeTransformGameObj = new GameObject();
        _runtimeTransformHandle = _runtimeTransformGameObj.AddComponent<RuntimeTransformHandle>();
        _runtimeTransformGameObj.layer = _runtimeTransformLayer;
        _runtimeTransformLayerMask = 1 << _runtimeTransformLayer;
        _runtimeTransformHandle.type = HandleType.POSITION;
        _runtimeTransformHandle.autoScale = true;
        _runtimeTransformHandle.autoScaleFactor = 1.0f;
        _runtimeTransformGameObj.SetActive(false);
    }

    // Maneja el resaltado de objetos seleccionables
    private void HandleHighlight()
    {
        if (_highlight != null)
        {
            ResetHighlightMaterial();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out _raycastHit))
        {
            _highlight = _raycastHit.transform;

            if (_highlight.CompareTag("Selectable") && _highlight != _selection)
            {
                ApplyHighlightMaterial();
            }
            else
            {
                _highlight = null;
            }
        }
    }

    // Restaura el material de resaltado original
    private void ResetHighlightMaterial()
    {
        _highlight.GetComponent<MeshRenderer>().sharedMaterial = _originalMaterial_highlight;
        _highlight = null;
    }

    // Aplica el material de resaltado al objeto
    private void ApplyHighlightMaterial()
    {
        if (_highlight.GetComponent<MeshRenderer>().material != _highlightMaterial)
        {
            _originalMaterial_highlight = _highlight.GetComponent<MeshRenderer>().material;
            _highlight.GetComponent<MeshRenderer>().material = _highlightMaterial;
        }
    }

    // Maneja la selección de objetos
    private void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            ApplyLayerToChildren(_runtimeTransformGameObj);

            if (Physics.Raycast(ray, out _raycastHit))
            {
                if (Physics.Raycast(ray, out _raycastHitHandle, Mathf.Infinity, _runtimeTransformLayerMask))
                {
                    // Handle runtime transform handle click
                }
                else if (_highlight)
                {
                    UpdateSelection();
                }
                else
                {
                    ClearSelection();
                }
            }
            else
            {
                ClearSelection();
            }
        }
    }

    // Actualiza la selección y maneja la interacción con la manija de transformación
    private void UpdateSelection()
    {
        if (_selection != null)
        {
            _selection.GetComponent<MeshRenderer>().material = _originalMaterial_selection;
        }

        _selection = _raycastHit.transform;

        if (_selection.GetComponent<MeshRenderer>().material != _selectionMaterial)
        {
            _originalMaterial_selection = _originalMaterial_highlight;
            _selection.GetComponent<MeshRenderer>().material = _selectionMaterial;
            SetupRuntimeTransformHandle();
        }

        _highlight = null;
    }

    // Limpia la selección actual
    private void ClearSelection()
    {
        if (_selection)
        {
            _selection.GetComponent<MeshRenderer>().material = _originalMaterial_selection;
            _selection = null;
            _runtimeTransformGameObj.SetActive(false);
        }
    }

    // Configura la manija de transformación en tiempo de ejecución
    private void SetupRuntimeTransformHandle()
    {
        _runtimeTransformHandle.target = _selection;
        _runtimeTransformGameObj.SetActive(true);
    }

    // Aplica la capa a los hijos de un GameObject
    private void ApplyLayerToChildren(GameObject parentGameObj)
    {
        foreach (Transform childTransform in parentGameObj.transform)
        {
            int layer = parentGameObj.layer;
            childTransform.gameObject.layer = layer;
            ApplyLayerToChildrenRecursive(childTransform);
        }
    }

    // Aplica la capa a los hijos de un GameObject de forma recursiva
    private void ApplyLayerToChildrenRecursive(Transform parentTransform)
    {
        foreach (Transform childTransform in parentTransform)
        {
            int layer = parentTransform.gameObject.layer;
            childTransform.gameObject.layer = layer;
            ApplyLayerToChildrenRecursive(childTransform);
        }
    }

    // Controla las teclas rápidas para la transformación
    private void HandleHotKeysForTransform()
    {
        if (_runtimeTransformGameObj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _runtimeTransformHandle.type = HandleType.POSITION;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _runtimeTransformHandle.type = HandleType.ROTATION;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _runtimeTransformHandle.type = HandleType.SCALE;
            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    _runtimeTransformHandle.space = HandleSpace.WORLD;
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    _runtimeTransformHandle.space = HandleSpace.LOCAL;
                }
            }
        }
    }
}
