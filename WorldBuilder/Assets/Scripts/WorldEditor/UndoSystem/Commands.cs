using UnityEngine;

public interface ICommand
{
    void Undo();
}

public class MoveCommand : ICommand
{
    private readonly Transform _transform;
    private readonly Vector3 _lastPosition;
    
    public MoveCommand(Transform transform, Vector3 lastPosition)
    {
        _transform = transform;
        _lastPosition = lastPosition;
    }
    
    public void Undo()
    {
        _transform.position = _lastPosition;
    }
}

public class RotateCommand : ICommand
{
    private readonly Transform _transform;
    private readonly Quaternion _lastRotation;
    
    public RotateCommand(Transform transform, Quaternion lastRotation)
    {
        _transform = transform;
        _lastRotation = lastRotation;
    }
    
    public void Undo()
    {
        _transform.rotation = _lastRotation;
    }
}

public class ScaleCommand : ICommand
{
    private readonly Transform _transform;
    private readonly Vector3 _lastScale;
    
    public ScaleCommand(Transform transform, Vector3 lastScale)
    {
        _transform = transform;
        _lastScale = lastScale;
    }
    
    public void Undo()
    {
        _transform.localScale = _lastScale;
    }
}

public class DeleteCommand : ICommand
{
    private readonly GameObject _gameObject;
    
    public DeleteCommand(GameObject gameObject)
    {
        _gameObject = gameObject;
        _gameObject.SetActive(false);
    }
    
    public void Undo()
    {
        _gameObject.SetActive(true);
    }
}
