using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int maxActionPoints;
    private int _actionPoints = 0;
    private int _health = 0;
    public int Health => _health;
    [SerializeField] private int _maxHealth = 0;
    public bool isPlayerDead;

    public void ChangeHealth(int amount)
    {
        if (_health + amount > _maxHealth)
            _health = _maxHealth;
        else if (_health + amount <= 0)
        {
            _health = 0;
            GameManager.Instance.GameOver();
        }
        else
        {
            _health += amount;
        }
        GameManager.Instance.uiManager.UpdatePlayerHealth();
    }

    public int ActionPoints {
        get
        {
            return _actionPoints;
        }
        set
        {
            _actionPoints = value;
            //Debug.Log("Action Points changed" + ActionPoints);
        }
        }
    void Awake()
    {
        RefreshActionPoints();
        _health = _maxHealth;
    }

    void Update()
    {
        if (!isPlayerDead)
        {
            if (GameManager.Instance.IsPlayerTurn)
            {
                CheckforMovement();
            }
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
            //Debug.LogWarning("Tile " + (transform.position.x + movement.x) + "," + (transform.position.z + movement.z) + " is already occupied or a wall");
        }
        else if (ActionPoints <= 0)
        {
            //Debug.LogWarning("Out Of ActionPoints");
        }
        else
        {
            Move(movement);
        }
    }
    private void Move(Vector3 movement)
    {
        var gm = GameManager.Instance;
        //clear previous position
        gm.gridHandler.SetIsNodeOccupied((int)transform.position.x,(int)transform.position.z, false);
        //move
        transform.position += movement;
        //set new position occupied
        gm.gridHandler.SetIsNodeOccupied((int)transform.position.x,(int)transform.position.z, true);
        //reduce action points by 1
        ActionPoints -= 1;
        gm.uiManager.UpdatePlayerActionText(ActionPoints);
    }
    public void IncreaseActionPoints(int amount)
    {
        maxActionPoints += amount;
        ActionPoints += amount;
        GameManager.Instance.uiManager.UpdatePlayerActionText(ActionPoints);
    }
    public void RefreshActionPoints()
    {
        ActionPoints = maxActionPoints;
        //Debug.Log("refreshed action points");
        GameManager.Instance.uiManager.UpdatePlayerActionText(ActionPoints);
    }
    public void ResetActionPoints()
    {
        ActionPoints = 0;
    }
    public bool CheckActionPointsReachedZero()
    {
        if (ActionPoints == 0)
            return true;
        return false;
    }
}
