using UnityEngine;

public enum GizmoType { Translation, Rotation, Scale }

public class Gizmo : MonoBehaviour
{
    public GizmoType type;
    public Vector3 direction;
}
