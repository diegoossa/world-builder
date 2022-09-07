using UnityEngine;

public class RaycastService : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CurrentRaycastTargetSO currentRaycastTarget;
    
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnPrimaryTouchStartedStarted;
    }
    
    private void OnDisable()
    {
        inputReader.PrimaryTouchStartedEvent -= OnPrimaryTouchStartedStarted;
    }

    private void OnPrimaryTouchStartedStarted(Vector2 position)
    {
        var ray = _mainCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out var hit, 50f))
        {
            Logger.Instance.Log($"HIT LAYER >> {LayerMask.LayerToName(hit.transform.gameObject.layer)}");
            currentRaycastTarget.SetCurrentTarget(hit.transform.gameObject.layer);
        }
    }

}
