using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectData
{
    public GameObject prefab;
    public Vector3 position;
    
    public ObjectData(GameObject prefab, Vector3 position)
    {
        this.prefab = prefab;
        this.position = position;
    }
}

[CreateAssetMenu(menuName = "Persistent/Session Data")]
public class CurrentSessionData : ScriptableObject
{
    public List<ObjectData> sessionData;

    public void AddObjectData(ObjectData objectData)
    {
        sessionData.Add(objectData);
    }
}
