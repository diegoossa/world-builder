using System;
using System.Collections;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    private TouchInput _touchInput;
    private Coroutine _zoomCoroutine;

    private void Awake()
    {
        _touchInput = new TouchInput();
    }

    private void OnEnable()
    {
        _touchInput.Enable();
    }

    private void OnDisable()
    {
        _touchInput.Disable();
    }

    private void Start()
    {
        _touchInput.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        _touchInput.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

    private void ZoomEnd()
    {
        _zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomStart()
    {
        StopCoroutine(_zoomCoroutine);
    }

    private IEnumerator ZoomDetection()
    {
        yield break;
        
    }
}
