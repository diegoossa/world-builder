using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // [SerializeField] private bool active;
    [SerializeField] private GameObject visual;
    [SerializeField] private Shader outlineShader;
    private Shader _originalShader;
    private Renderer _renderer;

    public void SetActive(bool value)
    {
        if (value)
        {
            OutlineEnable();
        }
        else
        {
            OutlineDisable();
        }
    }

    private void Start()
    {
        outlineShader = Shader.Find("Outline/Extrude Vertex/Standard");
        _renderer = visual.GetComponent<Renderer>();
        _originalShader = _renderer.material.shader;
    }

    private void OutlineEnable()
    {
        var materials = _renderer.materials;
        foreach (var material in materials)
            material.shader = outlineShader;
    }

    private void OutlineDisable()
    {
        var materials = _renderer.materials;
        foreach (var material in materials)
            material.shader = _originalShader;
    }
}