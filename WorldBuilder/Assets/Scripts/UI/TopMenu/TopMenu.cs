using UnityEngine;

public class TopMenu : MonoBehaviour
{
    [Header("Broadcasting On")] 
    [SerializeField]
    private VoidEventChannelSO undoChange;
    [SerializeField] 
    private VoidEventChannelSO clearObjects;
    
    public void UndoClicked()
    {
        if (undoChange)
            undoChange.RaiseEvent();
    }
    
    public void ClearClicked()
    {
        if (clearObjects)
            clearObjects.RaiseEvent();
    }
}
