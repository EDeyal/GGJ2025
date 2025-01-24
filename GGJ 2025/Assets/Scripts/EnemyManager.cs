using System.Collections;
using System.Collections.Generic;
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
}
