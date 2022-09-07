using UnityEngine;

public enum GizmoType
{
    Translation,
    Rotation,
    Scale
}

public class Gizmo : MonoBehaviour
{
    public GizmoType type;
    public Vector3 direction;

    public void ApplyMovement(Vector2 delta)
    {
        switch(type)
        {
            case GizmoType.Translation:
                transform.Translate(delta * direction);
                break;
            case GizmoType.Rotation:
                transform.Rotate(direction, delta.magnitude);
                break;
            case GizmoType.Scale:
                //transform.localScale
                break;
        }
    }
}
