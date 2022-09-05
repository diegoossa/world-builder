using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private FadeChannelSO fadeChannelSO;

    private void OnEnable()
    {
        fadeChannelSO.OnEventRaised += InitiateFade;
    }

    private void OnDisable()
    {
        fadeChannelSO.OnEventRaised -= InitiateFade;
    }

    private void InitiateFade(bool fadeIn, float duration, Color desiredColor)
    {
        //_imageComponent.DOBlendableColor(desiredColor, duration);
    }
}
