using UnityEngine;
using UnityEngine.EventSystems;

public class EditorCameraController : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private InputReader inputReader;

    [Header("Camera reference")] 
    [SerializeField] private Camera mainCamera;

    [Header("Panning")] 
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private Vector2 cameraBounds;

    [Header("Zoom")] 
    [SerializeField] private float minZoom = 30f;
    [SerializeField] private float maxZoom = 3f;
    
    [Header("Rotation")] 
    [SerializeField] private float rotationSpeed = 10f;

    [Space]
    [SerializeField] private int groundLayer;

    private Transform _cameraTransform;
    private Plane _groundPlane;
    private bool _isPinching;
    private bool _shouldPan;
    private bool _isUIFocus;
    
    private bool _isRotating;
    private Vector3 _pivotPoint;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            _cameraTransform = mainCamera.transform;
    }

    private void Start()
    {
        // Initialize Ground Plane
        _groundPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);

#if UNITY_EDITOR
        panSpeed *= 5;
        rotationSpeed *= 5;
#endif
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnPrimaryTouchStarted;
        inputReader.PrimaryTouchEndedEvent += OnPrimaryTouchEnded;
        inputReader.PrimaryTouchMovedEvent += OnPrimaryTouchMoved;
        inputReader.SecondaryTouchEvent += OnStartPinch;
        inputReader.StartPinchEvent += OnStartPinch;
        inputReader.StopPinchEvent += OnStopPinch;
        
#if UNITY_EDITOR
        inputReader.RightClickStartedEvent += OnRightClickStarted;
        inputReader.RightClickEndedEvent += OnRightClickEnded;
        inputReader.ScrollEvent += OnScroll;
#endif
    }

    private void OnDisable()
    {
        inputReader.PrimaryTouchStartedEvent -= OnPrimaryTouchStarted;
        inputReader.PrimaryTouchEndedEvent -= OnPrimaryTouchEnded;
        inputReader.PrimaryTouchMovedEvent -= OnPrimaryTouchMoved;
        inputReader.SecondaryTouchEvent -= OnStartPinch;
        inputReader.StartPinchEvent -= OnStartPinch;
        inputReader.StopPinchEvent -= OnStopPinch;
        
#if UNITY_EDITOR
        inputReader.RightClickStartedEvent -= OnRightClickStarted;
        inputReader.RightClickEndedEvent -= OnRightClickEnded;
        inputReader.ScrollEvent -= OnScroll;
#endif
    }

    private void Update()
    {
        _isUIFocus = EventSystem.current.IsPointerOverGameObject();
    }

    /// <summary>
    /// Check if we press the ground so we can pan
    /// </summary>
    /// <param name="touchPosition"></param>
    private void OnPrimaryTouchStarted(Vector2 touchPosition)
    {
        _shouldPan = true;
    }
    
    private void OnPrimaryTouchEnded(Vector2 position)
    {
        _shouldPan = false;
    }

    private void OnStartPinch() => _isPinching = true;

    private void OnStopPinch() => _isPinching = false;

    private void OnPrimaryTouchMoved(TouchData touchData)
    {
#if UNITY_EDITOR
        // Rotate just with Mouse
        if (_isRotating)
        {
            _cameraTransform.RotateAround(_pivotPoint, _groundPlane.normal, touchData.DeltaPosition.x * rotationSpeed * Time.deltaTime);
            return;
        }
#endif
        if (gameState.CurrentGameState != GameState.World)
            return;
        
        if (_isPinching || !_shouldPan || _isUIFocus)
            return;

        // Pan Movement
        var deltaMovement = GetWorldPositionDelta(touchData.Position, touchData.DeltaPosition);
        var cameraPositionCache = _cameraTransform.position;
        _cameraTransform.transform.Translate(deltaMovement * panSpeed * Time.deltaTime, Space.World);
        if (_cameraTransform.position.x < cameraBounds.x ||
            _cameraTransform.position.x > cameraBounds.y ||
            _cameraTransform.position.z < cameraBounds.x ||
            _cameraTransform.position.z > cameraBounds.y)
        {
            _cameraTransform.position = cameraPositionCache;
        }
    }

    private void OnStartPinch(TouchData primaryTouch, TouchData secondaryTouch)
    {
#if (UNITY_IOS || UNITY_ANDROID)
        if (!_isPinching || _isUIFocus)
            return;

        var currentPrimaryPosition = GetWorldPosition(primaryTouch.Position);
        var currentSecondaryPosition = GetWorldPosition(secondaryTouch.Position);
        var previousPrimaryPosition = GetWorldPosition(primaryTouch.Position - primaryTouch.DeltaPosition);
        var previousSecondaryPosition = GetWorldPosition(secondaryTouch.Position - secondaryTouch.DeltaPosition);

        // Calculate Zoom
        var zoomAmount = Vector3.Distance(currentPrimaryPosition, currentSecondaryPosition) /
                         Vector3.Distance(previousPrimaryPosition, previousSecondaryPosition);

        var cameraPositionCache = _cameraTransform.position;
        _cameraTransform.position = Vector3.LerpUnclamped(currentPrimaryPosition, cameraPositionCache, 1 / zoomAmount);

        // Check Zoom Bounds
        if (_cameraTransform.position.y > minZoom || _cameraTransform.position.y < maxZoom)
        {
            _cameraTransform.position = cameraPositionCache;
        }

        // Perform Camera Rotation
        _cameraTransform.RotateAround(currentPrimaryPosition, _groundPlane.normal,
            Vector3.SignedAngle(currentSecondaryPosition - currentPrimaryPosition,
                previousSecondaryPosition - previousPrimaryPosition, _groundPlane.normal) * rotationSpeed *
            Time.deltaTime);
#endif
    }
    
#if UNITY_EDITOR  
    private void OnRightClickStarted(Vector2 position)
    {
        _isRotating = true;
        _pivotPoint = GetWorldPosition(position);
    }
    
    private void OnRightClickEnded()
    {
        _isRotating = false;
        _pivotPoint = Vector3.zero;
    }

    private void OnScroll(float value)
    {
        var currentPosition = _cameraTransform.position;
        var cameraPositionCache = currentPosition;
        currentPosition += _cameraTransform.forward * value * 5f * Time.deltaTime;
        _cameraTransform.position = currentPosition;

        // Check Zoom Bounds
        if (_cameraTransform.position.y > minZoom || _cameraTransform.position.y < maxZoom)
            _cameraTransform.position = cameraPositionCache;
    }
#endif

    /// <summary>
    /// Get position in the ground plane
    /// </summary>
    /// <param name="screenPosition">Screen Position</param>
    /// <returns>Ground position</returns>
    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        var ray = mainCamera.ScreenPointToRay(screenPosition);
        if (_groundPlane.Raycast(ray, out var enter))
            return ray.GetPoint(enter);
        return Vector3.zero;
    }

    /// <summary>
    /// Get delta position from the ground plane
    /// </summary>
    /// <param name="position">Touch position</param>
    /// <param name="deltaPosition">Touch delta</param>
    /// <returns>Delta position in ground</returns>
    private Vector3 GetWorldPositionDelta(Vector2 position, Vector2 deltaPosition)
    {
        var previousRay = mainCamera.ScreenPointToRay(position - deltaPosition);
        var currentRay = mainCamera.ScreenPointToRay(position);
        if (_groundPlane.Raycast(previousRay, out var enterBefore) &&
            _groundPlane.Raycast(currentRay, out var enterNow))
            return previousRay.GetPoint(enterBefore) - currentRay.GetPoint(enterNow);

        return Vector3.zero;
    }
}