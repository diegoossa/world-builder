using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake()
    {
        if (Camera.main != null) 
            _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        var targetRotation = _cameraTransform.rotation;
        transform.LookAt(transform.position + targetRotation * Vector3.forward, targetRotation * Vector3.up);
    }
}
