using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>   
{
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public GridHandler gridHandler;
    [SerializeField] public UIManager uiManager;
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
            playerManager.player.RefreshActionPoints();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwapTurn();
        }
        if (IsPlayerTurn)
        {
            CheckAutoEndTurn();
        }
        else
        { 
            TryEnemyTurn();
        }
    }
    public void EndTurnButton()
    {
        if (IsPlayerTurn)
        {
            SwapTurn();
        }
    }
    void SwapTurn()
    {
        if (IsPlayerTurn)
        {
            //set to not player turn
            IsPlayerTurn = false;
            playerManager.player.ResetActionPoints();
            //enemy ui start turn
            uiManager.SwapTurn(false);
        }
        else
        { 
            //Set to player turn
            IsPlayerTurn=true;
            playerManager.player.RefreshActionPoints();
            //player ui start turn
            uiManager.SwapTurn(true);
        }
        uiManager.UpdatePlayerActionText(playerManager.player.ActionPoints);
    }
    void CheckAutoEndTurn()
    {
        if (playerManager.player.CheckActionPointsReachedZero())
        {
            Debug.Log("Player eneded his turn");
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
