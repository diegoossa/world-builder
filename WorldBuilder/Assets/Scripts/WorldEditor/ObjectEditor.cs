using System.Collections.Generic;
using UnityEngine;

public class ObjectEditor : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;
    
    [Header("Listening To")]
    [SerializeField] 
    private InputReader inputReader;
    [SerializeField] 
    private TransformEventChannelSO selectObject;
    [SerializeField] 
    private VoidEventChannelSO cancelSelectObject;
    [SerializeField] 
    private VoidEventChannelSO deleteObject;
    [SerializeField] 
    private VoidEventChannelSO undoLastChange;
    

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

    // Undo system
    private Stack<ICommand> _historyStack;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private Vector3 _lastScale;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _historyStack = new Stack<ICommand>();
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnTouchStarted;
        inputReader.PrimaryTouchEndedEvent += OnTouchEnded;
        inputReader.PrimaryTouchMovedEvent += OnTouchMoved;

        selectObject.OnEventRaised += OnSelectObject;
        cancelSelectObject.OnEventRaised += OnCancelSelect;
        deleteObject.OnEventRaised += OnDeleteObject;

        undoLastChange.OnEventRaised += OnUndoChange;
    }

    private void OnDisable()
    {
        inputReader.PrimaryTouchStartedEvent -= OnTouchStarted;
        inputReader.PrimaryTouchEndedEvent -= OnTouchEnded;
        inputReader.PrimaryTouchMovedEvent -= OnTouchMoved;
        
        selectObject.OnEventRaised -= OnSelectObject;
        cancelSelectObject.OnEventRaised -= OnCancelSelect;
        deleteObject.OnEventRaised -= OnDeleteObject;
        
        undoLastChange.OnEventRaised -= OnUndoChange;
    }

    private void OnTouchStarted(Vector2 touchPosition)
    {
        var ray = _mainCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out var hit, 100f, gizmoMask))
        {
            if (!hit.transform.TryGetComponent(out _currentGizmo)) 
                return;
            
            if (_currentGizmo != null) 
                gameState.UpdateGameState(GameState.Editing);
            
            switch (_currentGizmo.type)
            {
                case GizmoType.Translation:
                    _lastPosition = currentObject.position;
                    break;
                case GizmoType.Rotation:
                    _lastRotation = currentObject.rotation;
                    break;
                case GizmoType.Scale:
                    _lastScale = currentObject.localScale;
                    break;
            }
        }
    }

    private void OnTouchEnded(Vector2 touchPosition)
    {
        if (_currentGizmo == null) 
            return;
        
        switch (_currentGizmo.type)
        {
            case GizmoType.Translation:
                _historyStack.Push(new MoveCommand(currentObject, _lastPosition));
                break;
            case GizmoType.Rotation:
                _historyStack.Push(new RotateCommand(currentObject, _lastRotation));
                break;
            case GizmoType.Scale:
                _historyStack.Push(new ScaleCommand(currentObject, _lastScale));
                break;
        }
        _currentGizmo = null;
        gameState.ResetToPreviousGameState();
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
        var scaleFactor = _currentGizmo.direction * dot * movementSpeed * Time.deltaTime;
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
    
    private void OnDeleteObject()
    {
        if (currentObject)
        {
            _historyStack.Push(new DeleteCommand(currentObject.gameObject));
        }
    }
    
    private void OnUndoChange()
    {
        if (_historyStack.TryPop(out var command))
        {
            command.Undo();
        }
    }
}