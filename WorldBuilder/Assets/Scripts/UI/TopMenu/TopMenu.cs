using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : MonoBehaviour
{
    [Header("Broadcasting On")] [SerializeField]
    private VoidEventChannelSO undoChange;
    
    public void UndoClicked()
    {
        if (undoChange)
            undoChange.RaiseEvent();
    }
}
