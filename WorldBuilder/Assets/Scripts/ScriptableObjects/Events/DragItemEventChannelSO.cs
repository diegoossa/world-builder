using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Drag Item Event Channel")]
public class DragItemEventChannelSO : ScriptableObject
{
	public UnityAction<Vector2, GameObject> OnEventRaised;
	
	public void RaiseEvent(Vector2 position, GameObject obj)
	{
		OnEventRaised?.Invoke(position, obj);
	}
}
