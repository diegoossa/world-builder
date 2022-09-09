using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Create Object Event Channel")]
public class CreateObjectEventChannelSO : ScriptableObject
{
	public UnityAction<Vector2, GameObject> OnEventRaised;
	
	public void RaiseEvent(Vector2 position, GameObject obj)
	{
		OnEventRaised?.Invoke(position, obj);
	}
}
