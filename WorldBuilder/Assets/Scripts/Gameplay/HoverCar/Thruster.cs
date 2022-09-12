/* Script based on https://www.youtube.com/watch?v=r9OEZmbD9q0 */

using UnityEngine;

public class Thruster : MonoBehaviour
{
    [Header("Thruster Settings")] 
    [SerializeField] private float thrusterStrength;
    [SerializeField] private float thrusterDistance;
    
    [Header("Thrusters")] 
    [SerializeField] private Transform[] thrusters;

    private Rigidbody _rigidbody;
    private float _rigidBodyMass;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidBodyMass = _rigidbody.mass;
    }

    private void FixedUpdate()
    {
        foreach (var thruster in thrusters)
        {
            if (Physics.Raycast(thruster.position, -thruster.up, out var hit, thrusterDistance))
            {
                var distancePercentage = 1 - (hit.distance / thrusterDistance);
                var downwardForce = transform.up * (thrusterStrength * distancePercentage);
                downwardForce *= Time.deltaTime * _rigidBodyMass;
                _rigidbody.AddForceAtPosition(downwardForce, thruster.position);
            }
        }
    }
}
