using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealthRegeneration : WaveUpgrade
{
    [SerializeField] int _amount;
    public override void Activate()
    {
        GameManager.Instance.playerManager.player.IncreaseHealthRegeneration(_amount);
    }

    public override bool CheckIsAvailable()
    {
        return true;
    }

    public override string Description()
    {
        return "Increases health regeneration by: " + _amount;
    }
}
