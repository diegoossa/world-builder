using UnityEngine;

public class ObjectEditor : MonoBehaviour
{
    [Header("Listening")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameState gameState;

    [Header("Gizmo Settings")]
    [SerializeField] private LayerMask gizmoMask;
    [SerializeField] private float movementSpeed = 10f;

    [SerializeField] private Transform _currentObject;
    [SerializeField] private Gizmo _currentGizmo;
    private Camera _mainCamera;
    private Plane _groundPlane;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnTouchStarted;
        inputReader.PrimaryTouchEndedEvent += OnTouchEnded;
        inputReader.PrimaryTouchMovedEvent += OnTouchMoved;
        
        // Initialize Ground Plane
        _groundPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
    }

    private void OnDisable()
    {
        inputReader.PrimaryTouchStartedEvent -= OnTouchStarted;
        inputReader.PrimaryTouchEndedEvent -= OnTouchEnded;
        inputReader.PrimaryTouchMovedEvent -= OnTouchMoved;
    }

    private void OnTouchStarted(Vector2 touchPosition)
    {
        // if (gameState != GameState.Editing)
        //     return;

        var ray = _mainCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out var hit, 50f, gizmoMask))
        {
            Logger.Instance.Log("STARTED TOUCHING GIZMO");
            if (hit.transform.TryGetComponent(out _currentGizmo))
            {
                _currentObject = hit.transform;
            } 
        }
    }

    private void OnTouchEnded(Vector2 touchPosition)
    {
        Logger.Instance.Log("FINISH EDITION");
        _currentObject = null;
        _currentGizmo = null;
    }

    private void OnTouchMoved(TouchData touchData)
    {
        if (!_currentObject || !_currentGizmo) 
            return;

        var delta = new Vector3(touchData.DeltaPosition.x, touchData.DeltaPosition.y, touchData.DeltaPosition.y);
        var dot = Vector3.Dot(delta, _currentGizmo.direction);
        
        switch (_currentGizmo.type)
        {
            case GizmoType.Translation:
                _currentObject.Translate(dot * _currentGizmo.direction * Time.deltaTime * movementSpeed,
                    Space.World);
                break;
            case GizmoType.Rotation:
                var lookDirection = _currentObject.position - _mainCamera.transform.position;
                var angle = Vector3.SignedAngle(lookDirection, delta, _currentGizmo.direction);
                _currentObject.Rotate(_currentGizmo.direction, angle *  movementSpeed * Time.deltaTime);
                break;
            case GizmoType.Scale:
                var scale = Vector3.one * dot * movementSpeed * Time.deltaTime;
                Logger.Instance.Log($"SCALE{scale}");
                _currentObject.localScale += scale;
                break;
        }
    }
}