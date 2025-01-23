using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GridHandler gridHandler;
    private void Start()
    {
        gridHandler.SpawnFloor();
        playerManager.SpawnPlayer();
    }
}
