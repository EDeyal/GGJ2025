using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponForPlayer : WaveUpgrade
{
    static bool isUnlocked;
    public override void Activate()
    {
        GameManager.Instance.playerManager.UnlockSecondAbility();
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
}
