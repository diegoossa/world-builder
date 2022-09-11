using System;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;
    
    [Header("Listening To")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private DragItemEventChannelSO dragItem;
    
    [Header("Broadcasting On")]
    [SerializeField] private GameObjectEventChannelSO playerCreatedEvent;

    [Header("Dragging Settings")] 
    [SerializeField]
    private float minDistanceToCreate;

    private Camera _mainCamera;
    private Vector2 _startPosition;
    private GameObject _prefabToCreate;
    private Plane _groundPlane;
    private Transform _objectCreatedTransform;
    
    private bool _canDrag;
    private bool _objectCreated;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        
        // Initialize Ground Plane
        _groundPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchMovedEvent += OnTouchMoved;
        inputReader.PrimaryTouchEndedEvent += OnTouchEnded;
        
        dragItem.OnEventRaised += OnDraggingObject;
    }
    
    private void OnDisable()
    {
        inputReader.PrimaryTouchMovedEvent -= OnTouchMoved;
        inputReader.PrimaryTouchEndedEvent -= OnTouchEnded;
        
        dragItem.OnEventRaised -= OnDraggingObject;
    }

    private void OnDraggingObject(Vector2 startPosition, GameObject prefab)
    {
        _startPosition = startPosition;
        _prefabToCreate = prefab;
        _canDrag = true;
    }

    private void OnTouchMoved(TouchData touchData)
    {
        if (!_canDrag)
            return;

        if (_objectCreated)
        {
            // Drag Object
            var groundPosition = GetWorldPosition(touchData.Position);

            if (_objectCreatedTransform)
                _objectCreatedTransform.position = groundPosition;
        }
        else
        {
            // Check if create the object
            var yDistance = touchData.Position.y - _startPosition.y;
            if (yDistance >= minDistanceToCreate)
            {
                var currentObject = Instantiate(_prefabToCreate);

                // Check if object created is a Player
                if (currentObject.CompareTag("Player"))
                {
                    playerCreatedEvent.RaiseEvent(currentObject);
                }

                _objectCreatedTransform = currentObject.transform;
                _objectCreated = true;

                // Set gameState
                gameState.UpdateGameState(GameState.Creating);
            }
        }
    }
    
    private void OnTouchEnded(Vector2 touchPosition)
    {
        if (_objectCreated)
        {
            gameState.ResetToPreviousGameState();
            _objectCreated = false;
        }
        _canDrag = false;
    }
    
    /// <summary>
    /// Get position in the ground plane
    /// </summary>
    /// <param name="screenPosition">Screen Position</param>
    /// <returns>Ground position</returns>
    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        var ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (_groundPlane.Raycast(ray, out var enter))
            return ray.GetPoint(enter);
        return Vector3.zero;
    }
}
