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
        return true;
    }
}
