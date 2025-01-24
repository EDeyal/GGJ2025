using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseActionPointsUpgrade : WaveUpgrade
{
    [SerializeField] int _increaseAmount;
    public override void Activate()
    {
        GameManager.Instance.playerManager.player.IncreaseActionPoints(_increaseAmount);
    }

    public override bool CheckIsAvailable()
    {
        return true;
    }

    public override string Description()
    {
        return "Increase action point amount by " + _increaseAmount;
    }
}
