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
    public event UnityAction<Vector2> PrimaryTouchStartedEvent = delegate { };
    public event UnityAction<Vector2> PrimaryTouchEndedEvent = delegate { };
    public event UnityAction<TouchData> PrimaryTouchMovedEvent = delegate { };
    public event UnityAction<Vector2> PrimaryTapEvent = delegate { };
    public event UnityAction<TouchData, TouchData> SecondaryTouchEvent = delegate { };
    public event UnityAction StartPinchEvent = delegate { };
    public event UnityAction StopPinchEvent = delegate { };
    
    private TouchInput _touchInput;

    private void OnEnable()
    {
        if (_touchInput == null)
        {
            _touchInput = new TouchInput();
            _touchInput.Enable();

            _touchInput.Touch.PrimaryTouchContact.performed += _ =>  PrimaryTouchStarted();
            _touchInput.Touch.PrimaryTouchContact.canceled += _ => PrimaryTouchEnded();
            _touchInput.Touch.PrimaryTouchDelta.performed += _ => PrimaryTouchMove();
            _touchInput.Touch.SecondaryTouchPosition.performed += _ => StartPinch();
            _touchInput.Touch.SecondaryTouchContact.started += _ => StartPinchEvent.Invoke();
            _touchInput.Touch.SecondaryTouchContact.canceled += _ => StopPinchEvent.Invoke();
            _touchInput.Touch.PrimaryTap.performed += _ => PrimaryTap();
        }
    }

    private void OnDisable()
    {
        _touchInput.Disable();
    }
    
    private void PrimaryTap()
    {
        PrimaryTapEvent.Invoke(_touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>());
    }

    /// <summary>
    /// Panning movement
    /// </summary>
    private void PrimaryTouchMove()
    {
        PrimaryTouchMovedEvent.Invoke( new TouchData
        {
            Position = _touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>(),
            DeltaPosition = _touchInput.Touch.PrimaryTouchDelta.ReadValue<Vector2>()
        });
    }

    /// <summary>
    /// Zoom and Rotate
    /// </summary>
    private void StartPinch()
    {
        SecondaryTouchEvent.Invoke(
            new TouchData
            {
                Position = _touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>(),
                DeltaPosition = _touchInput.Touch.PrimaryTouchDelta.ReadValue<Vector2>()
            },
            new TouchData
            {
                Position = _touchInput.Touch.SecondaryTouchPosition.ReadValue<Vector2>(),
                DeltaPosition = _touchInput.Touch.SecondaryTouchDelta.ReadValue<Vector2>()
            });
    }
    
    private void PrimaryTouchStarted()
    {
        PrimaryTouchStartedEvent.Invoke(_touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>());
    }
    
    private void PrimaryTouchEnded()
    {
        PrimaryTouchEndedEvent.Invoke(_touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>());
    }
}