using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveUpgradeHandler : MonoBehaviour
{
    [SerializeField] List<WaveUpgrade> _goodUpgrades;
    [SerializeField] List<WaveUpgrade> _badUpgrades;
    [SerializeField] WaveUpgradeUI _option1;
    [SerializeField] WaveUpgradeUI _option2;

    public void ShowUpgrades(bool isTrue)
    {
        GameManager.Instance.uiManager.ShowWaveUpgrades(isTrue);
    }
    public void AssignUpgrades()
    {
        // Ensure there are enough upgrades available
        if (_goodUpgrades.Count < 2 || _badUpgrades.Count < 2)
        {
            Debug.LogError("Not enough upgrades in the lists!");
            return;
        }

        // Randomly pick two different good upgrades
        WaveUpgrade goodUpgrade1 = GetRandomUpgrade(_goodUpgrades);
        WaveUpgrade goodUpgrade2 = GetRandomUpgrade(_goodUpgrades, goodUpgrade1);

        // Randomly pick two different bad upgrades
        WaveUpgrade badUpgrade1 = GetRandomUpgrade(_badUpgrades);
        WaveUpgrade badUpgrade2 = GetRandomUpgrade(_badUpgrades, badUpgrade1);

        // Assign upgrades to options
        _option1.goodUpgrade = goodUpgrade1;
        _option1.badUpgrade = badUpgrade1;

        _option2.goodUpgrade = goodUpgrade2;
        _option2.badUpgrade = badUpgrade2;

        Debug.Log("Upgrades assigned!");

        _option1.UpdateUpgradesTexts();
        _option2.UpdateUpgradesTexts();
    }


    private WaveUpgrade GetRandomUpgrade(List<WaveUpgrade> upgradeList, WaveUpgrade exclude = null)
    {
        WaveUpgrade selectedUpgrade;
        int attempts = 0; // Prevent infinite loop

        do
        {
            selectedUpgrade = upgradeList[Random.Range(0, upgradeList.Count)];
            attempts++;

            if (attempts > upgradeList.Count * 2) // Safety limit
            {
                Debug.LogWarning("No available upgrades found.");
                return null; // Or handle fallback logic
            }
        }
        while (
            (exclude != null && selectedUpgrade == exclude) || // Ensure uniqueness
            !selectedUpgrade.CheckIsAvailable()               // Ensure availability
        );

        return selectedUpgrade;
    }
}
