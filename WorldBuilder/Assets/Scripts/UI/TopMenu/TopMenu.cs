using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    [Header("Listening To")] 
    [SerializeField] private GameObjectEventChannelSO playerCreatedEvent;
    [SerializeField] private VoidEventChannelSO playerDeletedEvent;
    
    [Header("Broadcasting On")] 
    [SerializeField]
    private VoidEventChannelSO undoChange;
    [SerializeField] 
    private VoidEventChannelSO clearObjects;
    [SerializeField] 
    private VoidEventChannelSO playStarted;
    [SerializeField] 
    private VoidEventChannelSO playStopped;

    [Header("Buttons")] 
    [SerializeField] private Button undoButton;
    [SerializeField] private Button clearAllButton;
    [SerializeField] private Button playSceneButton;
    [SerializeField] private Button stopPlayButton;

    private void OnEnable()
    {
        playerCreatedEvent.OnEventRaised += OnPlayerCreated;
        playerDeletedEvent.OnEventRaised += OnPlayerDeleted;
        
        undoButton.onClick.AddListener(UndoClicked);
        clearAllButton.onClick.AddListener(ClearClicked);
        playSceneButton.onClick.AddListener(PlayClicked);
        stopPlayButton.onClick.AddListener(StopClicked);
    }
    
    private void OnDisable()
    {
        playerCreatedEvent.OnEventRaised -= OnPlayerCreated;
        playerDeletedEvent.OnEventRaised -= OnPlayerDeleted;
        
        undoButton.onClick.RemoveListener(UndoClicked);
        clearAllButton.onClick.RemoveListener(ClearClicked);
        playSceneButton.onClick.RemoveListener(PlayClicked);
        stopPlayButton.onClick.RemoveListener(StopClicked);
    }

    private void OnPlayerCreated(GameObject value)
    {
        var canvasGroup = playSceneButton.GetComponent<CanvasGroup>();
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1f;
    }
    
    private void OnPlayerDeleted()
    {
        var canvasGroup = playSceneButton.GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0.25f;
    }

    private void UndoClicked()
    {
        if (undoChange)
            undoChange.RaiseEvent();
    }

    private void ClearClicked()
    {
        if (clearObjects)
            clearObjects.RaiseEvent();
    }

    private void PlayClicked()
    {
        if(playStarted)
            playStarted.RaiseEvent();

        undoButton.gameObject.SetActive(false);
        clearAllButton.gameObject.SetActive(false);
        playSceneButton.gameObject.SetActive(false);
        stopPlayButton.gameObject.SetActive(true);
    }
    
    private void StopClicked()
    {
        if(playStopped)
            playStopped.RaiseEvent();
        
        undoButton.gameObject.SetActive(true);
        clearAllButton.gameObject.SetActive(true);
        playSceneButton.gameObject.SetActive(true);
        stopPlayButton.gameObject.SetActive(false);
    }
}
