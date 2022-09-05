using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [Header("Camera reference")] [SerializeField]
    private Camera mainCamera;
    
    [Header("Zoom")] [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 30f;
    [SerializeField] private float maxZoom = 1f;

    [Header("Rotation")] [SerializeField] private float rotationSpeed = 2f;

    private bool _isZooming;
    private Transform _cameraTransform;
    private Plane _groundPlane;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null) 
            _cameraTransform = mainCamera.transform;
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchMovedEvent += OnPrimaryTouchMoved;
        inputReader.SecondaryTouchEvent += OnStartPinch;
        inputReader.StartZoomEvent += OnStartZoom;
        inputReader.StopZoomEvent += OnStopZoom;

        _groundPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
    }

    private void OnDisable()
    {
        inputReader.PrimaryTouchMovedEvent -= OnPrimaryTouchMoved;
        inputReader.SecondaryTouchEvent -= OnStartPinch;
        inputReader.StartZoomEvent -= OnStartZoom;
        inputReader.StopZoomEvent -= OnStopZoom;
    }

    private void OnStartZoom() => _isZooming = true;

    private void OnStopZoom() => _isZooming = false;

    private void OnPrimaryTouchMoved(TouchData primaryTouch)
    {
        if (!_isZooming)
        {
            var deltaMovement = GetWorldPositionDelta(primaryTouch.Position, primaryTouch.DeltaPosition);
            _cameraTransform.transform.Translate(deltaMovement, Space.World);
        }
    }

    private void OnStartPinch(TouchData primaryTouch, TouchData secondaryTouch)
    {
        if (!_isZooming)
            return;

        var currentPrimaryPosition = GetWorldPosition(primaryTouch.Position);
        var currentSecondaryPosition = GetWorldPosition(secondaryTouch.Position);
        var previousPrimaryPosition = GetWorldPosition(primaryTouch.Position - primaryTouch.DeltaPosition);
        var previousSecondaryPosition = GetWorldPosition(secondaryTouch.Position - secondaryTouch.DeltaPosition);

        var zoomAmount = Vector3.Distance(currentPrimaryPosition, currentSecondaryPosition) /
                         Vector3.Distance(previousPrimaryPosition, previousSecondaryPosition);

        if (zoomAmount is 0 or > 10)
            return;

        _cameraTransform.position =
            Vector3.LerpUnclamped(currentPrimaryPosition, _cameraTransform.position, 1 / zoomAmount);

        if (previousSecondaryPosition != currentSecondaryPosition)
        {
            _cameraTransform.RotateAround(currentPrimaryPosition, _groundPlane.normal,
                Vector3.SignedAngle(currentSecondaryPosition - currentPrimaryPosition,
                    previousSecondaryPosition - previousPrimaryPosition, _groundPlane.normal));
        }
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        var ray = mainCamera.ScreenPointToRay(screenPosition);
        if (_groundPlane.Raycast(ray, out var enter))
            return ray.GetPoint(enter);
        return Vector3.zero;
    }

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