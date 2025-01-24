using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    Player _player;
    public Player player { 
        get { return _player; }}

    public void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab, new Vector3(0,1,0),Quaternion.identity);
        if (GameManager.Instance.gridHandler.CheckIsNodeOccupied(0,0) == false)
        {
            GameManager.Instance.gridHandler.SetIsNodeOccupied(0,0, true);
        }
    }
}
