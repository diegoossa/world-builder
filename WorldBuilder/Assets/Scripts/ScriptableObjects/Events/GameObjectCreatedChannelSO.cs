using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameObject Created Event Channel")]
public class GameObjectCreatedChannelSO : ScriptableObject
{
	public UnityAction<GameObject, Transform> OnEventRaised;
	
	public void RaiseEvent(GameObject prefab, Transform transform)
	{
		OnEventRaised?.Invoke(prefab, transform);
	}
}
