using UnityEngine;

public class ObjectEditor : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    
    [Header("Listening To")]
    [SerializeField] 
    private InputReader inputReader;
    [SerializeField] 
    private TransformEventChannelSO selectObject;
    [SerializeField] 
    private VoidEventChannelSO cancelSelectObject;

    [Header("Gizmo Settings")]
    [SerializeField] private LayerMask gizmoMask;
    [SerializeField] private float movementSpeed = 10f;
    
    [Header("Selected Object")]
    [SerializeField] private Transform currentObject;

    [Header("Translation Settings")] 
    [SerializeField] 
    private Vector2 yLimit;
    [SerializeField]
    private Vector2 xLimit;
    [SerializeField]
    private Vector2 zLimit;
    
    [Header("Scale Settings")] 
    [SerializeField] 
    private Vector2 scaleLimit;
    
    private Gizmo _currentGizmo;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnTouchStarted;
        inputReader.PrimaryTouchEndedEvent += OnTouchEnded;
        inputReader.PrimaryTouchMovedEvent += OnTouchMoved;

        selectObject.OnEventRaised += OnSelectObject;
        cancelSelectObject.OnEventRaised += OnCancelSelect;
    }

    private void OnDisable()
    {
        inputReader.PrimaryTouchStartedEvent -= OnTouchStarted;
        inputReader.PrimaryTouchEndedEvent -= OnTouchEnded;
        inputReader.PrimaryTouchMovedEvent -= OnTouchMoved;
        
        selectObject.OnEventRaised -= OnSelectObject;
        cancelSelectObject.OnEventRaised -= OnCancelSelect;
    }

    private void OnTouchStarted(Vector2 touchPosition)
    {
        // if (gameState != GameState.Editing)
        //     return;

        var ray = _mainCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out var hit, 50f, gizmoMask))
        {
            hit.transform.TryGetComponent(out _currentGizmo);
        }
    }

    private void OnTouchEnded(Vector2 touchPosition)
    {
        _currentGizmo = null;
    }

    private void OnTouchMoved(TouchData touchData)
    {
        if (!currentObject || !_currentGizmo) 
            return;

        var delta = new Vector3(touchData.DeltaPosition.x, touchData.DeltaPosition.y, touchData.DeltaPosition.y);
        var dot = Vector3.Dot(delta, _currentGizmo.direction);
        
        switch (_currentGizmo.type)
        {
            case GizmoType.Translation:
                Translate(dot);
                break;
            case GizmoType.Rotation:
                Rotate(delta);
                break;
            case GizmoType.Scale:
                Scale(dot);
                break;
        }
    }

    private void Translate(float dot)
    {
        var movement = dot * _currentGizmo.direction * Time.deltaTime * movementSpeed;
        currentObject.Translate(movement, Space.World);
        var position = currentObject.position;
        position = new Vector3(
            Mathf.Clamp(position.x, xLimit.x, xLimit.y),
            Mathf.Clamp(position.y, yLimit.x, yLimit.y),
            Mathf.Clamp(position.z, zLimit.x, zLimit.y)
            );
        currentObject.position = position;
    }

    private void Rotate(Vector3 delta)
    {
        var lookDirection = _mainCamera.transform.position -  currentObject.position;
        var angle = Vector3.SignedAngle(lookDirection, delta, _currentGizmo.direction);
        currentObject.Rotate(_currentGizmo.direction, angle *  movementSpeed * Time.deltaTime);
    }

    private void Scale(float dot)
    {
        var scaleFactor = -_currentGizmo.direction * dot * movementSpeed * Time.deltaTime;
        var scale = currentObject.localScale + scaleFactor;
        scale = new Vector3(
            Mathf.Clamp(scale.x, scaleLimit.x, scaleLimit.y),
            Mathf.Clamp(scale.y, scaleLimit.x, scaleLimit.y),
            Mathf.Clamp(scale.z, scaleLimit.x, scaleLimit.y)
            );
        currentObject.localScale = scale;
    }
    
    private void OnSelectObject(Transform value)
    {
        currentObject = value.transform;
    }
    
    private void OnCancelSelect()
    {
        currentObject = null;
    }
}