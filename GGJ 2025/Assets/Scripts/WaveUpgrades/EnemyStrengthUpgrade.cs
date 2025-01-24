using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrengthUpgrade : WaveUpgrade
{
    [SerializeField] int _strengthIncreaseAmount;
    public override void Activate()
    {
        GameManager.Instance.enemyManager.IncreaseEnemyStrength(_strengthIncreaseAmount);
    }

    public override bool CheckIsAvailable()
    {
        return true;
    }

    public override string Description()
    {
        return "Increase enemy total strength by: " + _strengthIncreaseAmount;
    }
}
