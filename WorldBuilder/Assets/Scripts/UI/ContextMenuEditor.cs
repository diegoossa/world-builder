using System;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuEditor : MonoBehaviour
{
    [Header("Listening To")] 
    [SerializeField]
    private TransformEventChannelSO selectObject;
    [SerializeField] 
    private VoidEventChannelSO cancelSelectObject;

    [Header("Broadcasting On")] 
    [SerializeField]
    private ShowGizmoChannelSO showGizmoEditor;
    [SerializeField] 
    private VoidEventChannelSO hideGizmos;
    [SerializeField] 
    private VoidEventChannelSO deleteObject;
    [SerializeField] 
    private VoidEventChannelSO resetSelection;

    [Header("UI Buttons")] 
    [SerializeField]
    private Button translateButton;
    [SerializeField] 
    private Button rotateButton;
    [SerializeField] 
    private Button scaleButton;
    [SerializeField] 
    private Button backButton;
    [SerializeField] 
    private Button deleteButton;
    
    private enum ContextMenuState { Hidden, Main, Editing }
    
    [Header("State")] 
    [SerializeField] 
    private ContextMenuState currentState;

    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Edit = Animator.StringToHash("Edit");
    
    private Animator _animator;
    private GraphicRaycaster _raycaster;
    private Transform _targetTransform;
    private Transform _transform;
    private float _originalScale;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _raycaster = GetComponent<GraphicRaycaster>();

        _originalScale = transform.localScale.x;
    }

    private void OnEnable()
    {
        // Subscribe to Selection Events
        selectObject.OnEventRaised += OnSelectObject;
        cancelSelectObject.OnEventRaised += OnCancelSelect;

        // Subscribe to UI Events
        translateButton.onClick.AddListener(OnTranslateClicked);
        rotateButton.onClick.AddListener(OnRotateClicked);
        scaleButton.onClick.AddListener(OnScaleClicked);
        backButton.onClick.AddListener(OnBackClicked);
        deleteButton.onClick.AddListener(OnDeleteClicked);

        _transform = transform;
    }

    private void OnDisable()
    {
        selectObject.OnEventRaised -= OnSelectObject;
        cancelSelectObject.OnEventRaised -= OnCancelSelect;

        translateButton.onClick.RemoveListener(OnTranslateClicked);
        rotateButton.onClick.RemoveListener(OnRotateClicked);
        scaleButton.onClick.RemoveListener(OnScaleClicked);
        backButton.onClick.RemoveListener(OnBackClicked);
        deleteButton.onClick.RemoveListener(OnDeleteClicked);
    }

    private void Update()
    {
        if (_targetTransform)
        {
            _transform.position = _targetTransform.position;
            var targetScale = _targetTransform.localScale;
            var average = (targetScale.x + targetScale.y + targetScale.z) / 3f;
            _transform.localScale = Vector3.one * (average * _originalScale);;
        }
    }

    private void OnTranslateClicked()
    {
        if (showGizmoEditor)
            showGizmoEditor.RaiseEvent(GizmoType.Translation);

        currentState = ContextMenuState.Editing;
        _animator.SetBool(Edit, true);
    }

    private void OnRotateClicked()
    {
        if (showGizmoEditor)
            showGizmoEditor.RaiseEvent(GizmoType.Rotation);

        currentState = ContextMenuState.Editing;
        _animator.SetBool(Edit, true);
    }

    private void OnScaleClicked()
    {
        if (showGizmoEditor)
            showGizmoEditor.RaiseEvent(GizmoType.Scale);

        currentState = ContextMenuState.Editing;
        _animator.SetBool(Edit, true);
    }

    private void OnBackClicked()
    {
        if (currentState == ContextMenuState.Editing)
        {
            currentState = ContextMenuState.Main;
            _animator.SetBool(Edit, false);
        }
        else
        {
            ResetMenu();
        }
        
        if (hideGizmos)
            hideGizmos.RaiseEvent();
    }

    private void OnDeleteClicked()
    {
        if(deleteObject)
            deleteObject.RaiseEvent();
        
        ResetMenu();
        
        if (hideGizmos)
            hideGizmos.RaiseEvent();
    }

    private void OnCancelSelect()
    {
        ResetMenu();
        
        if (hideGizmos)
            hideGizmos.RaiseEvent();
    }

    private void ResetMenu()
    {
        currentState = ContextMenuState.Hidden;
        _animator.SetBool(Show, false);
        _animator.SetBool(Edit, false);
        _raycaster.enabled = false;

        _targetTransform = null;
        _transform.localScale = Vector3.one * _originalScale;
        
        if (resetSelection)
            resetSelection.RaiseEvent();
    }

    private void OnSelectObject(Transform value)
    {
        _targetTransform = value;
        
        currentState = ContextMenuState.Main;
        _animator.SetBool(Edit, false);
        _animator.SetBool(Show, true);
        
        _raycaster.enabled = true;
    }
}