using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealth : WaveUpgrade
{
    [SerializeField] int _amount;
    public override void Activate()
    {
        GameManager.Instance.playerManager.player.IncreaseMaxHealth(_amount);
    }

    public override bool CheckIsAvailable()
    {
        return true;
    }

    public override string Description()
    {
        return "Increase player health by: " + _amount;
    }
}
