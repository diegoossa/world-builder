using System;
using Cinemachine;
using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [Header("Camera reference")] [SerializeField]
    private Transform cameraTransform;

    [Header("Panning")] [SerializeField]
    private float panSpeed = 5f;

    [Header("Zoom")] [SerializeField]
    private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 30f;
    [SerializeField] private float maxZoom = 1f;

    [Header("Rotation")] [SerializeField]
    private float rotationSpeed = 2f;
    
    private bool _isZooming;
    private float _previousDistance;
    
    private void Awake()
    {
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchMovedEvent += OnPrimaryTouchMoved;
        inputReader.SecondaryTouchEvent += OnStartPinch;
        inputReader.StartZoomEvent += OnStartZoom;
        inputReader.StopZoomEvent += OnStopZoom;
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

    private void OnPrimaryTouchMoved(Vector2 deltaPosition)
    {
        if (!_isZooming)
        {
            cameraTransform.transform.Translate(
                -deltaPosition.x * panSpeed * Time.deltaTime, 0, 
                -deltaPosition.y * panSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnStartPinch(TouchData primaryTouch, TouchData secondaryTouch)
    {
        if(!_isZooming)
            return;
        
        var distance = Vector2.Distance(primaryTouch.Position, secondaryTouch.Position);
      
        // Zoom out
        if (distance > _previousDistance)
        {
            var targetPosition = cameraTransform.transform.position + cameraTransform.forward;
            cameraTransform.position =
                Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * zoomSpeed);
        }
        // Zoom in
        else if (distance < _previousDistance)
        {
            var targetPosition = cameraTransform.transform.position - cameraTransform.forward;
            cameraTransform.position =
                Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * zoomSpeed);
        }

        var rotationAngle = Vector3.Angle(
            secondaryTouch.Position - primaryTouch.Position,
            (secondaryTouch.Position - secondaryTouch.DeltaPosition) -
            (primaryTouch.Position - primaryTouch.DeltaPosition));
        
        Debug.Log($"ROTATION ANGLE >> {rotationAngle}");
        
        cameraTransform.RotateAround(
            transform.position,
            Vector3.up,
            rotationAngle* rotationSpeed * Time.deltaTime);

        _previousDistance = distance;
    }

    // private void OnStartPinch(TouchData primaryTouch, TouchData secondaryTouch)
    // {
    //     if (!_isZooming)
    //         return;
    //
    //     var currentPrimaryPosition = PlanePosition(primaryTouch.Position);
    //     var currentSecondaryPosition = PlanePosition(secondaryTouch.Position);
    //     var lastPrimaryPosition =  PlanePosition(primaryTouch.Position - primaryTouch.DeltaPosition);
    //     var lastSecondaryPosition = PlanePosition(secondaryTouch.Position - secondaryTouch.DeltaPosition);
    //
    //     var zoom = Vector3.Distance(currentPrimaryPosition, currentSecondaryPosition) /
    //                Vector3.Distance(-lastPrimaryPosition, lastSecondaryPosition);
    //
    //     if (zoom is 0 or > 10)
    //         return;
    //
    //     var camPositionBeforeAdjustment = mainCamera.transform.position;
    //     mainCamera.transform.position = Vector3.LerpUnclamped(currentPrimaryPosition,  mainCamera.transform.position, 1 / zoom);
    //
    //     if (mainCamera.transform.position.y > (_cameraStartPosition.y + minZoom))
    //     {
    //         mainCamera.transform.position = camPositionBeforeAdjustment;
    //     }
    //     if (mainCamera.transform.position.y < (_cameraStartPosition.y - maxZoom) || mainCamera.transform.position.y <= 1)
    //     {
    //         mainCamera.transform.position = camPositionBeforeAdjustment;
    //     }
    //     
    //     if (lastSecondaryPosition != currentSecondaryPosition)
    //     {
    //         mainCamera.transform.RotateAround(currentPrimaryPosition, Vector3.up,
    //             Vector3.SignedAngle(currentSecondaryPosition - currentPrimaryPosition,
    //                 lastSecondaryPosition - lastPrimaryPosition, Vector3.up));
    //     }
    // }
}