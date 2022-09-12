using System;
using UnityEngine;

/// <summary>
/// Initialize World State
/// </summary>
public class WorldBootstrap : MonoBehaviour
{
    [Header("Game Settings")] [SerializeField]
    private GameStateSO gameState;

    [SerializeField] private int targetFrameRate = 30;

    private void Start()
    {
        gameState.UpdateGameState(GameState.World);
        Application.targetFrameRate = targetFrameRate;
    }

    private void OnApplicationQuit()
    {
        Cleanup();
    }

#if UNITY_ANDROID || UNITY_IOS
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Cleanup();
    }

#endif

    // Cleanup deleted (disabled) items
    private void Cleanup()
    {
        // TODO: Create a Manager that references the created objects to to this more efficiently
        var interactableObjects = FindObjectsOfType<InteractableObject>(true);
        foreach (var obj in interactableObjects)
        {
            if (!obj.gameObject.activeSelf)
            {
                Destroy(obj.gameObject);
            }
        }

        // Call save on ES3
        ES3AutoSaveMgr.Instance.Save();
    }
}