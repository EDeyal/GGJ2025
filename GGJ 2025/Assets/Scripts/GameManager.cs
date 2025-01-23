using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>   
{
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public GridHandler gridHandler;
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
    }
}
