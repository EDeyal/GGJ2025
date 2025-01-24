using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageUpgrade : WaveUpgrade
{
    [SerializeField] int _enemyID;
    [SerializeField] int _damageAmount;

    public override void Activate()
    {
        GameManager.Instance.enemyManager.IncreaseEnemyDamage(_enemyID, _damageAmount);
    }

    public override bool CheckIsAvailable()
    {
        foreach (var enemyType in GameManager.Instance.enemyManager.EnemyTypes)
        {
            if(enemyType.enemyID == _enemyID)
            {
                return true;
            }
        }
        return false;
    }

    public override string Description()
    {
        return "Increase enemy no. " + _enemyID + " damage by " + _damageAmount;
    }
}
