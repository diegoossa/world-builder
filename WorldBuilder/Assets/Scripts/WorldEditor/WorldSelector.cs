using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSelector : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private Transform selectedObject;
    private Camera _mainCamera;

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

    private void OnPrimaryTap(Vector2 tapPosition)
    {
        // if (EventSystem.current.IsPointerOverGameObject()) 
        //     return;

        var ray = _mainCamera.ScreenPointToRay(tapPosition);
        if (Physics.Raycast(ray, out var hit, 50f, interactableMask))
        {
            // if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
            // {
            //     Logger.Instance.Log($"HIT UI");
            // }
            
            if (selectedObject != hit.transform)
                ClearSelection();
            
            selectedObject = hit.transform;
            Logger.Instance.Log($"HIT {selectedObject.name}");
            selectedObject.TryGetComponent<InteractableObject>(out var interactableObject);
            if (interactableObject)
            {
                interactableObject.SetActive(true);
            }
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
        
        Logger.Instance.Log("CLEAR SELECTION");
        selectedObject.TryGetComponent<InteractableObject>(out var interactableObject);
        if (interactableObject)
        {
            interactableObject.SetActive(false);
            selectedObject = null;
        }
    }
}