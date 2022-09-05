using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public struct TouchData
{
    public Vector2 Position;
    public Vector2 DeltaPosition;
}

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject
{
    public event UnityAction<Vector2> PrimaryTouchMovedEvent = delegate { };
    public event UnityAction<TouchData, TouchData> SecondaryTouchEvent = delegate { };
    public event UnityAction StartZoomEvent = delegate { };
    public event UnityAction StopZoomEvent = delegate { };

    private TouchInput _touchInput;

    private void OnEnable()
    {
        if (_touchInput == null)
        {
            _touchInput = new TouchInput();
            _touchInput.Enable();

            _touchInput.Touch.PrimaryTouchMove.performed += PrimaryTouchMove;
            _touchInput.Touch.SecondaryTouchPosition.performed += StartPinch;
            _touchInput.Touch.SecondaryTouchContact.started += _ => StartZoomEvent.Invoke();
            _touchInput.Touch.SecondaryTouchContact.canceled += _ => StopZoomEvent.Invoke();
        }
    }

    private void OnDisable()
    {
        _touchInput.Disable();
    }

    /// <summary>
    /// Panning movement
    /// </summary>
    /// <param name="context"></param>
    private void PrimaryTouchMove(InputAction.CallbackContext context)
    {
        PrimaryTouchMovedEvent.Invoke(context.ReadValue<Vector2>());
    }

    private void StartPinch(InputAction.CallbackContext context)
    {
        SecondaryTouchEvent.Invoke(
            new TouchData
            {
                Position = _touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>(),
                DeltaPosition = _touchInput.Touch.PrimaryTouchMove.ReadValue<Vector2>()
            },
            new TouchData
            {
                Position = _touchInput.Touch.SecondaryTouchPosition.ReadValue<Vector2>(),
                DeltaPosition = _touchInput.Touch.SecondaryTouchMove.ReadValue<Vector2>()
            });
    }
}