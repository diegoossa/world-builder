/* Script based on https://www.youtube.com/watch?v=r9OEZmbD9q0 */

using UnityEngine;
using UnityEngine.InputSystem;

public class HoverCarController : MonoBehaviour, InputControls.IGameActions
{
   [SerializeField] private float acceleration;
   [SerializeField] private float rotationRate;
   [SerializeField] private float turnRotationAngle;
   [SerializeField] private float turnRotationSeekSpeed;

   private float _rotationVelocity;
   private float _groundAngleVelocity;
   private Rigidbody _rigidbody;
   private float _rigidbodyMass;

   private float _horizontal;
   private float _vertical;

   private InputControls _gameInput;

   private void Awake()
   {
      _rigidbody = GetComponent<Rigidbody>();
      _rigidbodyMass = _rigidbody.mass;
   }

   private void OnEnable()
   {
      if (_gameInput == null)
      {
         _gameInput = new InputControls();
         _gameInput.Game.Enable();
         _gameInput.Game.SetCallbacks(this);
      }
   }
   
   private void OnDisable()
   {
      _gameInput.Game.Disable();
   }

   private void FixedUpdate()
   {
      // Check if touching ground
      if (Physics.Raycast(transform.position, -transform.up, 3f))
      {
         _rigidbody.drag = 1;
         var forwardForce = transform.forward * (acceleration * _vertical);
         forwardForce *= Time.deltaTime * _rigidbodyMass;
         _rigidbody.AddForce(forwardForce);
      }
      else
      {
         _rigidbody.drag = 0;
      }

      var turnTorque = Vector3.up * (rotationRate * _horizontal);
      turnTorque *= Time.deltaTime * _rigidbodyMass;
      _rigidbody.AddTorque(turnTorque);
      
      // Fake rotation
      var newRotation = transform.eulerAngles;
      newRotation.z = Mathf.SmoothDampAngle(newRotation.z, _horizontal * -turnRotationAngle, ref _rotationVelocity,
         turnRotationSeekSpeed);
      transform.eulerAngles = newRotation;
   }

   public void OnHorizontal(InputAction.CallbackContext context)
   {
      _horizontal = context.ReadValue<float>();
   }

   public void OnVertical(InputAction.CallbackContext context)
   {
      _vertical = context.ReadValue<float>();
   }
}
