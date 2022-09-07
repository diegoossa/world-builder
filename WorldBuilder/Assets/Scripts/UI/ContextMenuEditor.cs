using UnityEngine;
using UnityEngine.UI;

public class ContextMenuEditor : MonoBehaviour
{
    [Header("Listening To")] [SerializeField]
    private GameObjectEventChannelSO selectObject;

    [SerializeField] private VoidEventChannelSO cancelSelectObject;

    [Header("Broadcasting On")] [SerializeField]
    private ShowGizmoChannelSO showGizmoEditor;

    [SerializeField] private VoidEventChannelSO hideGizmo;

    [Header("UI Buttons")] [SerializeField]
    private Button translateButton;

    [SerializeField] private Button rotateButton;
    [SerializeField] private Button scaleButton;
    [SerializeField] private Button backButton;
    
    private enum ContextMenuState { Hidden, Main, Editing }
    
    [Header("State")] 
    [SerializeField] private ContextMenuState currentState;

    private Animator _animator;
    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Edit = Animator.StringToHash("Edit");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
    }

    private void OnDisable()
    {
        selectObject.OnEventRaised -= OnSelectObject;
        cancelSelectObject.OnEventRaised -= OnCancelSelect;

        translateButton.onClick.RemoveListener(OnTranslateClicked);
        rotateButton.onClick.RemoveListener(OnRotateClicked);
        scaleButton.onClick.RemoveListener(OnScaleClicked);
        backButton.onClick.RemoveListener(OnBackClicked);
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
            showGizmoEditor.RaiseEvent(GizmoType.Translation);

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
            currentState = ContextMenuState.Hidden;
            _animator.SetBool(Show, false);
        }
        
        if (hideGizmo)
            hideGizmo.RaiseEvent();
    }

    private void OnCancelSelect()
    {
        currentState = ContextMenuState.Hidden;
        _animator.SetBool(Show, false);
        _animator.SetBool(Edit, false);
        if (hideGizmo)
            hideGizmo.RaiseEvent();
    }

    private void OnSelectObject(GameObject obj)
    {
        transform.position = obj.transform.position;
        currentState = ContextMenuState.Main;
        _animator.SetBool(Edit, false);
        _animator.SetBool(Show, true);
    }
}