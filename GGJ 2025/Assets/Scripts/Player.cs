using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int actionPoints;
    void Start()
    {

    }

    void Update()
    {
        if (GameManager.Instance.IsPlayerTurn)
        {
            CheckforMovement();
        }
    }
    void CheckforMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            TryMove(new Vector3(0, 0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            TryMove(new Vector3(0, 0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            TryMove(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            TryMove(new Vector3(1, 0, 0));
        }
    }

    void TryMove(Vector3 movement)
    {
        if (GameManager.Instance.gridHandler.CheckIsNodeOccupied((int)(transform.position.x + movement.x), (int)(transform.position.z + movement.z)))
        {
            Debug.LogWarning("Tile " + (transform.position.x + movement.x) + "," + (transform.position.z + movement.z) + " is already occupied or a wall");
        }
        else if (actionPoints <= 0)
        {
            Debug.LogWarning("Out Of ActionPoints");
        }
        else
        {
            Move(movement);
        }
    }
    private void Move(Vector3 movement)
    {
        //clear previous position
        GameManager.Instance.gridHandler.SetIsNodeOccupied((int)transform.position.x,(int)transform.position.z, false);
        //move
        transform.position += movement;
        //set new position occupied
        GameManager.Instance.gridHandler.SetIsNodeOccupied((int)transform.position.x,(int)transform.position.z, true);
        //reduce action points by 1
        actionPoints--;
    }
}
