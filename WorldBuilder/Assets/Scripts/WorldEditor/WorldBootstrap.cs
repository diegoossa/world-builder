using UnityEngine;

/// <summary>
/// Initialize World State
/// </summary>
public class WorldBootstrap : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private int targetFrameRate = 30;
    
    private void Start()
    {
        gameState.UpdateGameState(GameState.World);
        Application.targetFrameRate = targetFrameRate;
    }
}
