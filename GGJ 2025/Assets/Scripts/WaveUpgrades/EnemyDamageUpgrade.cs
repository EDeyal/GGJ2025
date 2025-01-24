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
        return true;
    }

    public override string Description()
    {
        return "Increase enemy no. " + _enemyID + " damage by " + _damageAmount;
    }
}
