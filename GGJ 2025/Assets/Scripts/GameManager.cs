using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>   
{
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public GridHandler gridHandler;
    public bool IsPlayerTurn = true;
    private void Start()
    {
        gridHandler.SpawnFloor();
        playerManager.SpawnPlayer();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerManager.player.actionPoints++;
        }
        CheckAutoEndTurn();
        TryEnemyTurn();
    }
    void SwapTurn()
    {
        if (IsPlayerTurn)
        {
            //enemy start turn
            IsPlayerTurn = false;
        }
        else
        { 
            //player start turn
            IsPlayerTurn=true;
        }
    }
    void CheckAutoEndTurn()
    {
        if (playerManager.player.actionPoints == 0)
        { 
            //player auto turn end
            SwapTurn();
        }
    }
    void TryEnemyTurn()
    {
        if (IsPlayerTurn)
            return;
        //enemy turn
    }
}
