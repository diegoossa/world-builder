using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject
{
    public event UnityAction<Vector2> PrimaryTouchStartedEvent = delegate { };
    public event UnityAction<Vector2> PrimaryTouchMovedEvent = delegate { };
    public event UnityAction<Vector2> SecondaryTouchEvent = delegate { };
    
    private TouchInput _touchInput;
    
    private void OnEnable()
    {
        if (_touchInput == null)
        {
            _touchInput = new TouchInput();
            _touchInput.Enable();

            _touchInput.Touch.TouchMove.started += PrimaryTouchMove;
            _touchInput.Touch.PrimaryTouchContact.started += PrimaryTouchStart;
        }
    }
    
    private void OnDisable()
    {
        _touchInput.Disable();
    }

    private void PrimaryTouchStart(InputAction.CallbackContext context)
    {
        PrimaryTouchStartedEvent.Invoke(_touchInput.Touch.PrimaryTouchPosition.ReadValue<Vector2>());
    }
    
    private void PrimaryTouchEnd(InputAction.CallbackContext context)
    {
        PrimaryTouchStartedEvent.Invoke(context.ReadValue<Vector2>());
    }
    
    private void PrimaryTouchMove(InputAction.CallbackContext context)
    {
        PrimaryTouchMovedEvent.Invoke(context.ReadValue<Vector2>());
    }
}
