using UnityEngine;
using UnityEngine.EventSystems;

public class MenuObject : MonoBehaviour, IPointerDownHandler
{
    [Header("Broadcasting On")]
    [SerializeField] 
    private DragItemEventChannelSO dragItem;
    [SerializeField] 
    private GameObject prefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(dragItem)
            dragItem.RaiseEvent(eventData.position, prefab);
    }
}
