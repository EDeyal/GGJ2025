using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>   
{
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public GridHandler gridHandler;
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public UIManager uiManager;
    [SerializeField] public WaveUpgradeHandler waveUpgradeHandler;
    public bool IsPlayerTurn = true;
    public int currentWave = 0;
    private void Start()
    {
        gridHandler.SpawnFloor();
        playerManager.SpawnPlayer();
        uiManager.UpdatePlayerHealth();
        gridHandler.UpdateEnemySpawnPositions();
        enemyManager.SpawnEnemies();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
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
            //Preform enemy turn
            EnemyTurn();
        }
        else
        { 
            //Set to player turn
            IsPlayerTurn=true;
            playerManager.StartPlayerTurn();
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
    private IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(1);
        //sort enemies by distance from player
        enemyManager.SortEnemyList();
        //grant them action points
        enemyManager.RefreshActionPoints();
        bool didAttack = enemyManager.EnemyAttack(); // Modify EnemyAttack to return a bool
        if (didAttack)
        {
            yield return new WaitForSeconds(2); // Wait only if an attack occurred
        }
        //move if can
        enemyManager.EnemyMovement();
        yield return new WaitForSeconds(1);
        SwapTurn();
    }
        void EnemyTurn()
    {
        StartCoroutine(EnemyTurnRoutine());
    }
    public void RestartGame()
    {
        playerManager.player.isPlayerDead = false;
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        uiManager.GameOver();
        playerManager.player.isPlayerDead = true;
    }
    public void WaveEnded()
    {
        //set up wave upgrades for level
        StartCoroutine(WaitForEnemyDeath());
        waveUpgradeHandler.AssignUpgrades();
        waveUpgradeHandler.ShowUpgrades(true);
    }
    IEnumerator WaitForEnemyDeath()
    {
        yield return new WaitForSeconds(2);

    }
    public void WaveUpgradeSelected()
    {
        waveUpgradeHandler.ShowUpgrades(false);
        gridHandler.UpdateEnemySpawnPositions();
        enemyManager.SpawnEnemies();
        playerManager.player.RefreshActionPoints();
        playerManager.player.RegenerateHealth();
    }
}