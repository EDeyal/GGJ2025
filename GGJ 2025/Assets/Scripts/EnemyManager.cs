using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy basicEnemyPrefab;
    [SerializeField] int StartingEnemyStrength = 1;
    int _currentEnemyStrength;
    int _enemyStrengthToSpawn;
    List<Enemy> _enemyList;
    List<Enemy> _enemyTypes;
    public List<Enemy> EnemyTypes => _enemyTypes;
    private void Awake()
    {
        _enemyList = new List<Enemy>();
        _enemyTypes = new List<Enemy>();
        var enemy = Instantiate(basicEnemyPrefab,transform);
        _enemyTypes.Add(enemy);
        _currentEnemyStrength = StartingEnemyStrength;
    }
    public void IncreaseEnemyDamage(int ID, int damageAmount)
    {
        foreach (var enemy in _enemyTypes)
        {
            if (enemy.enemyID == ID)
            { 
                enemy.IncreaseDamage(damageAmount);
            }
        }
    }
    public void IncreaseEnemyHealth(int ID, int healthAmount)
    {
        foreach (var enemy in _enemyTypes)
        {
            if (enemy.enemyID == ID)
            {
                enemy.IncreaseHealth(healthAmount);
            }
        }
    }
    public void AddEnemyType(Enemy enemy)
    {
        var newEnemy = Instantiate(enemy, transform);
        _enemyTypes.Add(newEnemy);
    }
    public void IncreaseEnemyStrength(int amount)
    { 
        _currentEnemyStrength += amount;
    }
    public void SpawnEnemies()
    {
        GameManager.Instance.currentWave += 1;
        GameManager.Instance.uiManager.UpdateWave();
        _enemyStrengthToSpawn = _currentEnemyStrength;
        while (_enemyStrengthToSpawn > 0)
        {
            int randomIndex = Random.Range(0, _enemyTypes.Count);
            if (_enemyTypes[randomIndex].enemyStrength <= _enemyStrengthToSpawn)
            {
                _enemyStrengthToSpawn -= _enemyTypes[randomIndex].enemyStrength;
                SpawnEnemy(_enemyTypes[randomIndex]);
            }
        }
    }
    
    void SpawnEnemy(Enemy enemyPrefab)
    {
        GridNode node = GameManager.Instance.gridHandler.GetSpawnableEnemyLocation();
        node.isOccupied = true;
        Enemy enemy = Instantiate(enemyPrefab, new Vector3(node.xPos, 1, node.yPos), new Quaternion(0,180,0,0));
        _enemyList.Add(enemy);
    }
    private void PlaySpawnEnemySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.Play();
        }
    }
    public void RefreshActionPoints()
    {
        foreach (Enemy enemy in _enemyList)
        { 
            enemy.RefreshActionPoints();
        }
    }
    public bool CheckForUnpreformedActions()
    {
        bool anyHasActionPoints = false;
        foreach (Enemy enemy in _enemyList)
        {
            if (enemy.ActionPoints != 0)
            { 
                anyHasActionPoints = true;
            }
        }
        return anyHasActionPoints;
    }

    public bool EnemyAttack()
    {
        bool didAttack = false;
        Vector3 playerPos = GameManager.Instance.playerManager.player.transform.position;

        foreach (Enemy enemy in _enemyList)
        {
            // Check if the enemy is within attack range
            if (Vector3.Distance(enemy.transform.position, playerPos) <= enemy.attackRange)
            {
                didAttack = true;
                StartCoroutine(AttackEnemy(enemy, playerPos));
            }
        }

        return didAttack;
    }
    private IEnumerator AttackEnemy(Enemy enemy, Vector3 playerPos)
    {
        // Rotate towards the player
        Vector3 directionToPlayer = (playerPos - enemy.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate the enemy towards the player
        while (Quaternion.Angle(enemy.transform.rotation, targetRotation) > 1f)
        {
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
            yield return null;
        }

        // Now that the enemy is facing the player, attack
        StartCoroutine(AttackAnimation(enemy, directionToPlayer));


        // (Optional) Simulate the attack (you can modify this part to suit your needs)
        GameManager.Instance.playerManager.player.ChangeHealth(-enemy.attackDamage);
    }
    private void PlayAttackSound(Enemy enemy)
    {
        // Play the sound effect for attack
        // Example: Assuming the enemy has an AudioSource attached
        AudioSource audioSource = enemy.gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.Play();
        }
    }
    private IEnumerator AttackAnimation(Enemy enemy, Vector3 directionToPlayer)
    {
        Vector3 initialPosition = enemy.transform.position;
        Vector3 attackPosition = initialPosition + directionToPlayer * 0.5f; // Move 0.5 units towards the player

        // Move towards the player
        float attackSpeed = 2f; // Adjust this value to control the speed of movement
        float elapsedTime = 0f;
        while (elapsedTime < 0.2f) // Adjust the duration for the forward movement
        {
            enemy.transform.position = Vector3.Lerp(initialPosition, attackPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime * attackSpeed;
            yield return null;
        }

        // Snap to attack position
        enemy.transform.position = attackPosition;
        // Play attack sound here
        PlayAttackSound(enemy);
        // Move back to the original position
        elapsedTime = 0f;
        while (elapsedTime < 0.2f) // Adjust the duration for the backward movement
        {
            enemy.transform.position = Vector3.Lerp(attackPosition, initialPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime * attackSpeed;
            yield return null;
        }

        // Snap back to the original position
        enemy.transform.position = initialPosition;
    }
    public void EnemyMovement()
    {
        Vector3 playerPos = GameManager.Instance.playerManager.player.transform.position;
        foreach (Enemy enemy in _enemyList)
        {
            if (enemy.ActionPoints == 0)
            {
                continue;
            }
            if (Vector3.Distance(enemy.transform.position, playerPos) > enemy.attackRange)
            {
                //need to move
                Vector3 direction = playerPos - enemy.transform.position;
                direction.Normalize();
                direction.x = (direction.x > 0 ? Mathf.Ceil(direction.x) : Mathf.Floor(direction.x));
                direction.z = (direction.z > 0 ? Mathf.Ceil(direction.z) : Mathf.Floor(direction.z));

                int randomIndex = Random.Range(0, 2);
                if (randomIndex == 0)
                { 
                    if (direction.x != 0)
                    {
                        if (!GameManager.Instance.gridHandler.CheckIsNodeOccupied((int)(enemy.transform.position.x + direction.x), (int)(enemy.transform.position.z)))
                        {
                            enemy.TryMove(new Vector3(direction.x, 0, 0));
                        }
                    }
                    if (direction.z != 0)
                    {
                        if (!GameManager.Instance.gridHandler.CheckIsNodeOccupied((int)(enemy.transform.position.x), (int)(enemy.transform.position.z + direction.z)))
                        {
                            enemy.TryMove(new Vector3(0, 0, direction.z));
                        }
                    }
                }
                else if (randomIndex == 1)
                {
                    if (direction.z != 0)
                    {
                        if (!GameManager.Instance.gridHandler.CheckIsNodeOccupied((int)(enemy.transform.position.x), (int)(enemy.transform.position.z + direction.z)))
                        {
                            enemy.TryMove(new Vector3(0, 0, direction.z));
                        }
                    }
                    if (direction.x != 0)
                    {
                        if (!GameManager.Instance.gridHandler.CheckIsNodeOccupied((int)(enemy.transform.position.x + direction.x), (int)(enemy.transform.position.z)))
                        {
                            enemy.TryMove(new Vector3(direction.x, 0, 0));
                        }
                    }
                }
                //enemy.TryMove();
            }
        }
    }

    public void SortEnemyList()
    {
        var playerPos = GameManager.Instance.playerManager.player.transform.position;
        List<Enemy> enemiesToSort = _enemyList;
        enemiesToSort.Sort((a, b) =>
            {
                float distanceA = Vector3.Distance(playerPos, a.transform.position);
                float distanceB = Vector3.Distance(playerPos, b.transform.position);
                return distanceA.CompareTo(distanceB);
            });
        _enemyList = enemiesToSort;
    }
    public void HitEnemy(Vector2 enemyPos, int damage)
    {
        Enemy enemy = GetEnemyFromPos(enemyPos);
        enemy.ChangeHealth(-damage);
        //Debug.Log("EnemyIsHit");
    }
    Enemy GetEnemyFromPos(Vector2 enemyPos)
    {
        foreach (var enemy in _enemyList)
        {
            if (enemy.transform.position.x == enemyPos.x && enemy.transform.position.z == enemyPos.y)
            {
                return enemy;
            }
        }
        return null;
    }
    public void KillEnemy(Enemy enemy)
    {
        //deathEffect in enemy location
        _enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
        CheckIfWaveIsOver();
    }
    void CheckIfWaveIsOver()
    {
        if (_enemyList.Count <= 0)
        {
            GameManager.Instance.WaveEnded();
        }
    }
}
