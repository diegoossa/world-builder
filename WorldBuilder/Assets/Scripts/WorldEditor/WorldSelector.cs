using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSelector : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] private InputReader inputReader;

    [Header("Broadcasting On")] 
    [SerializeField] private GameObjectEventChannelSO selectObjectChannel;
    [SerializeField] private VoidEventChannelSO cancelSelectObjectChannel;

    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private Transform selectedObject;
    
    private Camera _mainCamera;
    private bool _isUIFocus;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputReader.PrimaryTapEvent += OnPrimaryTap;
    }

    private void OnDisable()
    {
        inputReader.PrimaryTapEvent -= OnPrimaryTap;
    }

    private void Update()
    {
        _isUIFocus = EventSystem.current.IsPointerOverGameObject();
    }

    private void OnPrimaryTap(Vector2 tapPosition)
    {
        if (_isUIFocus)
            return;
        
        var ray = _mainCamera.ScreenPointToRay(tapPosition);
        if (Physics.Raycast(ray, out var hit, 50f, interactableMask))
        {
            if (selectedObject != hit.transform)
                ClearSelection();
            
            selectedObject = hit.transform;
            selectedObject.TryGetComponent<InteractableObject>(out var interactableObject);
            if (interactableObject)
            {
                interactableObject.SetActive(true);
            }
            
            if(selectObjectChannel)
                selectObjectChannel.RaiseEvent(hit.transform.gameObject);
        }
        else
        {
            ClearSelection();
        }
    }

    private void ClearSelection()
    {
        if (!selectedObject)
            return;
        
        selectedObject.TryGetComponent<InteractableObject>(out var interactableObject);
        if (interactableObject)
        {
            interactableObject.SetActive(false);
            selectedObject = null;
        }
        
        if(cancelSelectObjectChannel)
            cancelSelectObjectChannel.RaiseEvent();
    }
}