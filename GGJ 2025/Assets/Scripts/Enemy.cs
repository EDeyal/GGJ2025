using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] public int enemyID;
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
    public void IncreaseHealth(int amount)
    {
        _maxHealth += amount;
    }
    public void IncreaseDamage(int amount)
    {
        attackDamage += amount;
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
        var gm = GameManager.Instance;
        // Clear previous position
        gm.gridHandler.SetIsNodeOccupied((int)transform.position.x, (int)transform.position.z, false);

        // Calculate target position
        Vector3 targetPosition = transform.position + movement;

        // Start coroutine to move smoothly
        StartCoroutine(SmoothMoveAndRotate(targetPosition,movement, 0.5f)); // 0.5f = movement duration

        // Set new position occupied after moving
        gm.gridHandler.SetIsNodeOccupied((int)targetPosition.x, (int)targetPosition.z, true);

        // Reduce action points by 1
        _actionPoints -= 1;
    }
    private IEnumerator SmoothMoveAndRotate(Vector3 targetPosition, Vector3 movement, float duration)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        // Calculate target rotation based on movement direction
        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate position over time
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // Interpolate rotation over time
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position and rotation are set accurately
        transform.position = targetPosition;
        transform.rotation = targetRotation;
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
