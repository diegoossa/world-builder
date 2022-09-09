using UnityEngine;

/// <summary>
/// Initialize World State
/// </summary>
public class WorldBootstrap : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;
    
    private void Start()
    {
        gameState.UpdateGameState(GameState.World);
    }
}
