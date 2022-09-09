using UnityEngine;

public class BottomMenu : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;
    
    private void Start()
    {
        
    }

    public void OnBeginDrag()
    {
        gameState.UpdateGameState(GameState.ScrollingMenu);
    }

    public void OnEndDrag()
    {
        gameState.ResetToPreviousGameState();
    }
}
