using Cinemachine;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] private VoidEventChannelSO playStarted;
    [SerializeField] private VoidEventChannelSO playStopped;
    [SerializeField] private GameObjectEventChannelSO playerCreated;

    [Header("Camera Reference")] [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private void OnEnable()
    {
        playStarted.OnEventRaised += OnPlayStarted;
        playStopped.OnEventRaised += OnPlayStopped;
        playerCreated.OnEventRaised += OnPlayerCreated;
    }
    
    private void OnDisable()
    {
        playStarted.OnEventRaised -= OnPlayStarted;
        playStopped.OnEventRaised -= OnPlayStopped;
        playerCreated.OnEventRaised += OnPlayerCreated;
    }

    private void OnPlayerCreated(GameObject player)
    {
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }

    private void OnPlayStarted()
    {
        virtualCamera.gameObject.SetActive(true);
    }

    private void OnPlayStopped()
    {
        virtualCamera.gameObject.SetActive(false);
    }
}
