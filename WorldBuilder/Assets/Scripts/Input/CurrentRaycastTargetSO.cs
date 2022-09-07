using UnityEngine;

public enum RaycastTarget 
{
    UI,
    Interactable, 
    Ground,
    Gizmo
}

[CreateAssetMenu(fileName = "CurrentRaycastTarget", menuName = "Game/Target")]
public class CurrentRaycastTargetSO : ScriptableObject
{
    public RaycastTarget currentTarget;

    public void SetCurrentTarget(int layer)
    {
        currentTarget = layer switch
        {
            5 => RaycastTarget.UI,
            6 => RaycastTarget.Interactable,
            7 => RaycastTarget.Ground,
            8 => RaycastTarget.Gizmo,
            _ => currentTarget
        };
    }
}
