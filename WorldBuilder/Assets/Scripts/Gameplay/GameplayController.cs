using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;

    [Header("Listening To")] 
    [SerializeField] private VoidEventChannelSO playStarted;
    [SerializeField] private VoidEventChannelSO playStopped;
}
