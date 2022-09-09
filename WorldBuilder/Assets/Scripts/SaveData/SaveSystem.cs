using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [Header("Listening To")] [SerializeField]
    private GameObjectCreatedChannelSO gameObjectCreated;

    [Header("Session Data")] [SerializeField]
    private CurrentSessionData currentSessionData;

    private void OnEnable()
    {
        gameObjectCreated.OnEventRaised += OnGameObjectCreated;
    }
    
    private void OnDisable()
    {
        gameObjectCreated.OnEventRaised -= OnGameObjectCreated;
    }

    private void OnGameObjectCreated(GameObject prefab, Transform instanceTransform)
    {
        currentSessionData.AddObjectData(new ObjectData(prefab, instanceTransform.position));
    }
}
