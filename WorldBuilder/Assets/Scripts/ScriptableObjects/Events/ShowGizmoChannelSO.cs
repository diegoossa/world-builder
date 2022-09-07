using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Gizmo Event Channel")]
public class ShowGizmoChannelSO : ScriptableObject
{
	public UnityAction<GizmoType> OnEventRaised;
	
	public void RaiseEvent(GizmoType value)
	{
		OnEventRaised?.Invoke(value);
	}
}
