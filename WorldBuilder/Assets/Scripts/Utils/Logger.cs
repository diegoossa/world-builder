using System.Collections;
using InputSamples;
using TMPro;
using UnityEngine;

public class Logger : Singleton<Logger>
{
    private TextMeshProUGUI _loggerText;
    private Coroutine _currentCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _loggerText = GetComponent<TextMeshProUGUI>();
    }

    public void Log(string message)
    {
        _loggerText.text = message + "\n" + _loggerText.text;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        
        _currentCoroutine = StartCoroutine(ClearLogs());
    }

    private IEnumerator ClearLogs()
    {
        yield return new WaitForSeconds(2);
        _loggerText.text = "";
    }
}