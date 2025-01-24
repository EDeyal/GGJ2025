using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUpgradeUI : MonoBehaviour
{
    public WaveUpgrade goodUpgrade;
    [SerializeField] TextMeshProUGUI _goodUpgradeText;
    public WaveUpgrade badUpgrade;
    [SerializeField] TextMeshProUGUI _badUpgradeText;

    public void UpdateUpgradesTexts()
    {
        _goodUpgradeText.text = goodUpgrade.Description();
        _badUpgradeText.text = badUpgrade.Description();
    }
    public void UpgradeSelected()
    {
        goodUpgrade.Activate();
        badUpgrade.Activate();
        GameManager.Instance.WaveUpgradeSelected();
    }


}
