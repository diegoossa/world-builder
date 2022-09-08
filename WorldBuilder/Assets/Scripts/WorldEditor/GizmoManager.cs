using System;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    [Header("Gizmos")] 
    [SerializeField] private GameObject translateGizmo;
    [SerializeField] private GameObject rotateGizmo;
    [SerializeField] private GameObject scaleGizmo;
    
    [Header("Listening To")]
    [SerializeField] 
    private ShowGizmoChannelSO showGizmoEditor;
    [SerializeField] 
    private VoidEventChannelSO hideGizmos;
    [SerializeField]
    private TransformEventChannelSO selectObject;
    [SerializeField] 
    private VoidEventChannelSO cancelSelectObject;
    
    private Transform _targetTransform;
    
    private void OnEnable()
    {
        showGizmoEditor.OnEventRaised += OnShowGizmo;
        hideGizmos.OnEventRaised += OnHideGizmos;
        
        selectObject.OnEventRaised += OnSelectObject;
        cancelSelectObject.OnEventRaised += OnCancelSelect;
    }
    
    private void OnDisable()
    {
        showGizmoEditor.OnEventRaised -= OnShowGizmo;
        hideGizmos.OnEventRaised -= OnHideGizmos;
        
        selectObject.OnEventRaised -= OnSelectObject;
        cancelSelectObject.OnEventRaised -= OnCancelSelect;
    }

    private void Update()
    {
        if (_targetTransform)
        {
            transform.position = _targetTransform.position;
        }
    }

    private void OnShowGizmo(GizmoType type)
    {
        switch (type)
        {
            case GizmoType.Translation:
                translateGizmo.SetActive(true);
                break;
            case GizmoType.Rotation:
                rotateGizmo.SetActive(true);
                break;
            case GizmoType.Scale:
                scaleGizmo.SetActive(true);
                break;
        }
    }
    
    private void OnHideGizmos()
    {
        translateGizmo.SetActive(false);
        rotateGizmo.SetActive(false);
        scaleGizmo.SetActive(false);
    }
    
    private void OnSelectObject(Transform value)
    {
        _targetTransform = value;
    }

    private void OnCancelSelect()
    {
        _targetTransform = null;
    }
}
