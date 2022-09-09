using UnityEngine;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour, IPointerDownHandler
{
    [Header("Broadcasting On")]
    [SerializeField] 
    private CreateObjectEventChannelSO createObject;
    [SerializeField] 
    private GameObject prefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(createObject)
            createObject.RaiseEvent(eventData.position, prefab);
    }
}
