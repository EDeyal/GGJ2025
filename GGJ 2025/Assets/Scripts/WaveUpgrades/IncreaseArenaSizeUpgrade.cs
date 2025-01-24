using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseArenaSizeUpgrade : WaveUpgrade
{
    [SerializeField] int _gridXIncrease;
    [SerializeField] int _gridYIncrease;
    public override void Activate()
    {
        GameManager.Instance.gridHandler.IncreaseGridSize(_gridXIncrease, _gridYIncrease);
    }

    public override bool CheckIsAvailable()
    {
        return true;
    }
}
