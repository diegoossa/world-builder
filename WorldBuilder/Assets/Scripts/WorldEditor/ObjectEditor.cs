using UnityEngine;

public class ObjectEditor : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameState gameState;

    [SerializeField] private LayerMask gizmoMask;

    private Transform _currentObject;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnTouchStarted;
        inputReader.PrimaryTouchEndedEvent += OnTouchEnded;
        inputReader.PrimaryTouchMovedEvent += OnTouchMoved;
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
            _currentObject = hit.transform;
        }
    }

    private void OnTouchEnded(Vector2 touchPosition)
    {
        Logger.Instance.Log("FINISH EDITION");
        _currentObject = null;
    }

    private void OnTouchMoved(TouchData touchData)
    {
        if (!_currentObject) return;
        _currentObject.Translate( touchData.DeltaPosition.normalized * Vector2.up * Time.deltaTime, Space.World);
    }
}