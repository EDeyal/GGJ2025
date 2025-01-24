using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthUpgrade : WaveUpgrade
{
    [SerializeField] int _enemyID;
    [SerializeField] int _healthAmount;
    public override void Activate()
    {
        GameManager.Instance.enemyManager.IncreaseEnemyHealth(_enemyID,_healthAmount);
    }

    public override bool CheckIsAvailable()
    {
        foreach (var enemyType in GameManager.Instance.enemyManager.EnemyTypes)
        {
            if (enemyType.enemyID == _enemyID)
            {
                return true;
            }
        }
        return false;
    }

    public override string Description()
    {
        return "Increase enemy no. " + _enemyID + " health by " + _healthAmount;
    }
}