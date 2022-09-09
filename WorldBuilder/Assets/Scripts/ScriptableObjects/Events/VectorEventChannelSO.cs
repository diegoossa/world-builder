using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Vector Event Channel")]
public class VectorEventChannelSO : ScriptableObject
{
    public UnityAction<Vector3> OnEventRaised;
	
    public void RaiseEvent(Vector3 value)
    {
        OnEventRaised?.Invoke(value);
    }
}