using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] public int enemyStrength = 1;
   [SerializeField] public int maxActionPoints = 1;
   private int _actionPoints;
   public int ActionPoints => _actionPoints;
   [SerializeField] public int attackRange = 1;
   [SerializeField] public int attackDamage = 1;
   private int _health;
   [SerializeField] private int _maxHealth;

    private void Awake()
    {
        RefreshActionPoints();
        _health = _maxHealth;
    }
    public void RefreshActionPoints()
    {
        _actionPoints = maxActionPoints;
        //Debug.Log("refreshed action points");
    }
    public void TryMove(Vector3 movement)
    {
        if (GameManager.Instance.gridHandler.CheckIsNodeOccupied((int)(transform.position.x + movement.x), (int)(transform.position.z + movement.z)))
        {
            //Debug.LogWarning("Tile " + (transform.position.x + movement.x) + "," + (transform.position.z + movement.z) + " is already occupied or a wall");
        }
        else if (_actionPoints <= 0)
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
        Debug.Log(gameObject.name + "EnemyMoves");
        var gm = GameManager.Instance;
        //clear previous position
        gm.gridHandler.SetIsNodeOccupied((int)transform.position.x, (int)transform.position.z, false);
        //move
        transform.position += movement;
        //set new position occupied
        gm.gridHandler.SetIsNodeOccupied((int)transform.position.x, (int)transform.position.z, true);
        //reduce action points by 1
        _actionPoints -= 1;
    }
    public void ChangeHealth(int amount)
    {
        if (_health + amount > _maxHealth)
            _health = _maxHealth;
        else if (_health + amount <= 0)
        {
            _health = 0;
            GameManager.Instance.enemyManager.KillEnemy(this);
        }
        else
        {
            _health += amount;
        }
        GameManager.Instance.uiManager.UpdatePlayerHealth();
    }
    private void OnDestroy()
    {
        GameManager.Instance.gridHandler.UnoccupyTile(new Vector2(transform.position.x,transform.position.z));
    }
}
