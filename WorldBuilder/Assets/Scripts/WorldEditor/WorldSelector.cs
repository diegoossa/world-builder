using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSelector : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] private InputReader inputReader;

    [SerializeField] private VoidEventChannelSO resetSelection;

    [Header("Broadcasting On")] 
    [SerializeField] private TransformEventChannelSO selectObjectChannel;
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
        resetSelection.OnEventRaised += ClearSelection;
    }

    private void OnDisable()
    {
        inputReader.PrimaryTapEvent -= OnPrimaryTap;
        resetSelection.OnEventRaised -= ClearSelection;
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
        if (Physics.Raycast(ray, out var hit, 100f, interactableMask))
        {
            if (selectedObject != hit.transform)
                ClearSelection();
            
            selectedObject = hit.transform;
            if (selectedObject.TryGetComponent<InteractableObject>(out var interactableObject))
            {
                interactableObject.Select(true);
            }
            
            if(selectObjectChannel)
                selectObjectChannel.RaiseEvent(hit.transform);
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
        
        if (selectedObject.TryGetComponent<InteractableObject>(out var interactableObject))
        {
            interactableObject.Select(false);
            selectedObject = null;
        }
        
        if(cancelSelectObjectChannel)
            cancelSelectObjectChannel.RaiseEvent();
    }
}