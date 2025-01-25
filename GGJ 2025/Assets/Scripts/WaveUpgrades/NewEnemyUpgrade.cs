using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyUpgrade : WaveUpgrade
{
    public static bool isUnlocked;
    [SerializeField] Enemy enemyPrefab;

    public override void Activate()
    {
        GameManager.Instance.enemyManager.AddEnemyType(enemyPrefab);
        isUnlocked = true;
    }

    public override bool CheckIsAvailable()
    {
        if (isUnlocked)
        {
            return false;
        }
        return true;
    }

    public override string Description()
    {
        return "A new enemy type appears";
    }
}
