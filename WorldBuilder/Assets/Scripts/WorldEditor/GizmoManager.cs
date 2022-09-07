using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    [Header("Gizmos")] 
    [SerializeField] private GameObject translateGizmo;
    [SerializeField] private GameObject rotateGizmo;
    [SerializeField] private GameObject scaleGizmo;
    
    [Header("Listening To")]
    [SerializeField] private ShowGizmoChannelSO showGizmoEditor;
    [SerializeField] private VoidEventChannelSO hideGizmos;
    

    private void OnEnable()
    {
        showGizmoEditor.OnEventRaised += OnShowGizmo;
        hideGizmos.OnEventRaised += OnHideGizmos;
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
}
