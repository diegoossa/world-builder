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

#if UNITY_EDITOR
    //Mouse Only
    public event UnityAction<Vector2> RightClickStartedEvent = delegate { };
    public event UnityAction RightClickEndedEvent = delegate { };
    public event UnityAction<float> ScrollEvent = delegate { };
#endif

    private InputControls _inputControls;

    private void OnEnable()
    {
        if (_inputControls == null)
        {
            _inputControls = new InputControls();
            _inputControls.Enable();

            _inputControls.WorldEditor.PrimaryTouchContact.performed += _ => OnPrimaryTouchStarted();
            _inputControls.WorldEditor.PrimaryTouchContact.canceled += _ => OnPrimaryTouchEnded();
            _inputControls.WorldEditor.PrimaryTouchDelta.performed += _ => OnPrimaryTouchMove();
            _inputControls.WorldEditor.PrimaryTap.performed += _ => OnPrimaryTap();
            _inputControls.WorldEditor.SecondaryTouchPosition.performed += _ => OnStartPinch();
            _inputControls.WorldEditor.SecondaryTouchContact.started += _ => StartPinchEvent.Invoke();
            _inputControls.WorldEditor.SecondaryTouchContact.canceled += _ => StopPinchEvent.Invoke();

#if UNITY_EDITOR
            _inputControls.WorldEditor.RightClick.performed += _ => OnRightClick();
            _inputControls.WorldEditor.RightClick.canceled += _ => RightClickEndedEvent.Invoke();
            _inputControls.WorldEditor.Scroll.performed += OnScroll;
#endif
        }
    }

    private void OnDisable()
    {
        _inputControls.Disable();
    }

    private void OnPrimaryTap()
    {
        PrimaryTapEvent.Invoke(_inputControls.WorldEditor.PrimaryTouchPosition.ReadValue<Vector2>());
    }

    /// <summary>
    /// Panning movement
    /// </summary>
    private void OnPrimaryTouchMove()
    {
        PrimaryTouchMovedEvent.Invoke(new TouchData
        {
            Position = _inputControls.WorldEditor.PrimaryTouchPosition.ReadValue<Vector2>(),
            DeltaPosition = _inputControls.WorldEditor.PrimaryTouchDelta.ReadValue<Vector2>()
        });
    }

    /// <summary>
    /// Zoom and Rotate
    /// </summary>
    private void OnStartPinch()
    {
        SecondaryTouchEvent.Invoke(
            new TouchData
            {
                Position = _inputControls.WorldEditor.PrimaryTouchPosition.ReadValue<Vector2>(),
                DeltaPosition = _inputControls.WorldEditor.PrimaryTouchDelta.ReadValue<Vector2>()
            },
            new TouchData
            {
                Position = _inputControls.WorldEditor.SecondaryTouchPosition.ReadValue<Vector2>(),
                DeltaPosition = _inputControls.WorldEditor.SecondaryTouchDelta.ReadValue<Vector2>()
            });
    }

    private void OnPrimaryTouchStarted()
    {
        PrimaryTouchStartedEvent.Invoke(_inputControls.WorldEditor.PrimaryTouchPosition.ReadValue<Vector2>());
    }

    private void OnPrimaryTouchEnded()
    {
        PrimaryTouchEndedEvent.Invoke(_inputControls.WorldEditor.PrimaryTouchPosition.ReadValue<Vector2>());
    }

#if UNITY_EDITOR
    private void OnRightClick()
    {
        RightClickStartedEvent.Invoke(_inputControls.WorldEditor.PrimaryTouchPosition.ReadValue<Vector2>());
    }
    
    private void OnScroll(InputAction.CallbackContext ctx)
    {
        ScrollEvent.Invoke(ctx.ReadValue<float>());
    }
#endif
}