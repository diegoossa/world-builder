using UnityEngine;
using UnityEngine.EventSystems;

public class EditorCameraController : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private InputReader inputReader;

    [Header("Camera reference")] [SerializeField]
    private Camera mainCamera;

    [Header("Panning")] [SerializeField] private float panSpeed = 10f;
    [SerializeField] private Vector2 cameraBounds;

    [Header("Zoom")] [SerializeField] private float minZoom = 30f;
    [SerializeField] private float maxZoom = 3f;

    [Header("Rotation")] [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private int groundLayer;

    private Transform _cameraTransform;
    private bool _isPinching;
    private Plane _groundPlane;
    private bool _shouldPan;
    private bool _isUIFocus;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            _cameraTransform = mainCamera.transform;
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnPrimaryTouchStarted;
        inputReader.PrimaryTouchMovedEvent += OnPrimaryTouchMoved;
        inputReader.SecondaryTouchEvent += OnStartPinch;
        inputReader.StartPinchEvent += OnStartPinch;
        inputReader.StopPinchEvent += OnStopPinch;

        // Initialize Ground Plane
        _groundPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);

#if UNITY_EDITOR
        panSpeed *= 5;
#endif
    }

    private void OnDisable()
    {
        inputReader.PrimaryTouchStartedEvent -= OnPrimaryTouchStarted;
        inputReader.PrimaryTouchMovedEvent -= OnPrimaryTouchMoved;
        inputReader.SecondaryTouchEvent -= OnStartPinch;
        inputReader.StartPinchEvent -= OnStartPinch;
        inputReader.StopPinchEvent -= OnStopPinch;
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
        if (gameState.CurrentGameState != GameState.World)
            return;

        var ray = mainCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out var hit, 100f))
        {
            _shouldPan = hit.transform.gameObject.layer == groundLayer;
        }
    }

    private void OnStartPinch() => _isPinching = true;

    private void OnStopPinch() => _isPinching = false;

    private void OnPrimaryTouchMoved(TouchData touchData)
    {
        if (_isPinching || !_shouldPan || gameState.CurrentGameState != GameState.World || _isUIFocus)
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
        if (!_isPinching || _isUIFocus)
            return;

        var currentPrimaryPosition = GetWorldPosition(primaryTouch.Position);

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR

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

#else
        _cameraTransform.RotateAround(currentPrimaryPosition, _groundPlane.normal, primaryTouch.DeltaPosition.x * rotationSpeed * Time.deltaTime);
#endif
    }

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