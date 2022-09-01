using System;
using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private Camera _mainCamera;

    [Header("Panning Motion")]
    [SerializeField]
    private float panSpeed = 5f;
    [SerializeField] private bool _shouldMove;

    [SerializeField] private LayerMask interactableLayer;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        inputReader.PrimaryTouchStartedEvent += OnPrimaryTouchStart;
        inputReader.PrimaryTouchMovedEvent += OnPrimaryTouchMoved;
        _mainCamera = Camera.main;
    }

    private void OnDisable()
    {
        inputReader.PrimaryTouchStartedEvent -= OnPrimaryTouchStart;
        inputReader.PrimaryTouchMovedEvent -= OnPrimaryTouchMoved;
    }

    private void OnPrimaryTouchStart(Vector2 position)
    {
        Debug.Log("START TOUCH");
        // Raycast to check if we should pan or interact
        var ray = _mainCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Debug.Log("Interactable");
                _shouldMove = false;
            }
            else
            {
                Debug.Log("Not Interactable");
                _shouldMove = true;
            }
        }
    }

    private void OnPrimaryTouchMoved(Vector2 deltaPosition)
    {
        //Debug.Log($"POS >> {position} // DELTA >> {deltaPosition}");
        if (_shouldMove)
        {
            transform.Translate(-deltaPosition.x * panSpeed * Time.deltaTime, 0, -deltaPosition.y * panSpeed * Time.deltaTime);
        }
    }
}