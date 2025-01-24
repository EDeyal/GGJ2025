using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy basicEnemyPrefab;
    [SerializeField] int StartingEnemyStrength = 1;
    int _currentEnemyStrength;
    int _enemyStrengthToSpawn;
    List<Enemy> _enemyList;
    List<Enemy> _enemyTypes;
    private void Awake()
    {
        _enemyList = new List<Enemy>();
        _enemyTypes = new List<Enemy>();
        _enemyTypes.Add(basicEnemyPrefab);
        _currentEnemyStrength = StartingEnemyStrength;
    }
    public void SpawnEnemies()
    {
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
        Enemy enemy = Instantiate(enemyPrefab, new Vector3(node.xPos, 1, node.yPos), Quaternion.identity);
        _enemyList.Add(enemy);
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
    public void EnemyAttack()
    {
        Vector3 playerPos = GameManager.Instance.playerManager.player.transform.position;
        foreach (Enemy enemy in _enemyList)
        { 
            if (Vector3.Distance(enemy.transform.position, playerPos) <= enemy.attackRange)
            {
                Debug.Log(enemy.gameObject.name + "AttacksPlayer");
            }
        }
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

}
