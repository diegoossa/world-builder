using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeController : MonoBehaviour
{
    [SerializeField] 
    private FadeChannelSO fadeChannelSO;
    [SerializeField] 
    private Image image;

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
        image.DOBlendableColor(desiredColor, duration);
    }
}
