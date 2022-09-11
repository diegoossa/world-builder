using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Listening To")] 
    [SerializeField]
    private VoidEventChannelSO clearAllObjects;
    
    [Header("Interactable Settings")]
    [SerializeField] 
    private GameObject visual;
    public int id;
    
    [Header("Outline Shader")]
    [SerializeField] 
    private Shader outlineShader;
    
    private Shader _originalShader;
    private Renderer _renderer;

    public void Select(bool value)
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

    private void OnEnable()
    {
        clearAllObjects.OnEventRaised += OnClearAllObjects;
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
    
    
    private void OnClearAllObjects()
    {
        // TODO: Create Command for this
        gameObject.SetActive(false);
    }
}