using UnityEngine;

public enum GameState
{
    Loading,
    Editing
}

public class GameStateSO : ScriptableObject
{
    public GameState CurrentGameState => currentGameState;

    [Header("Game states")] 
    [SerializeField] 
    private GameState currentGameState;
    [SerializeField] 
    private GameState previousGameState;
    
    public void UpdateGameState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;

        previousGameState = currentGameState;
        currentGameState = newGameState;
    }

    public void ResetToPreviousGameState()
    {
        if (previousGameState == currentGameState)
            return;
        
        (previousGameState, currentGameState) = (currentGameState, previousGameState);
    }
}