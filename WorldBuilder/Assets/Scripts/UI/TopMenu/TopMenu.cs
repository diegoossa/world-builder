using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    [Header("Broadcasting On")] 
    [SerializeField]
    private VoidEventChannelSO undoChange;
    [SerializeField] 
    private VoidEventChannelSO clearObjects;

    [Header("Buttons")] 
    [SerializeField] private Button undoButton;
    [SerializeField] private Button clearAllButton;

    private void OnEnable()
    {
        undoButton.onClick.AddListener(UndoClicked);
        clearAllButton.onClick.AddListener(ClearClicked);
    }
    
    private void OnDisable()
    {
        undoButton.onClick.RemoveListener(UndoClicked);
        clearAllButton.onClick.RemoveListener(ClearClicked);
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
}
