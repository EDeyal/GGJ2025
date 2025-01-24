using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerActionPoints;
    [SerializeField] TextMeshPro playerHealthText;
    [SerializeField] GameObject playerTurn;
    [SerializeField] GameObject enemyTurn;
    [SerializeField] GameObject gameOver;
    [SerializeField] AbilityUiHandler abilityUIPrefab;
    [SerializeField] GameObject abilitiesUiHolder;
    [SerializeField] List<AbilityUiHandler> abilitiesUI;
    [SerializeField] GameObject NotEnoughActionPoints;
    [SerializeField] GameObject CanNotMove;
    [SerializeField] TextMeshProUGUI WaveCounterText;
    [SerializeField] GameObject WaveUpgradesHolder;

    public void UpdatePlayerActionText(int currentAmount)
    {
        playerActionPoints.text = currentAmount.ToString();
    }

    public void SwapTurn(bool isPlayerTurn)
    {
        GameObject go;
        if (isPlayerTurn)
        {
            go = playerTurn;
        }
        else
        {
            go = enemyTurn;
        }
            StartCoroutine(Popup(go));
    }

    IEnumerator Popup(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        go.SetActive(false);
    }
    IEnumerator Popup(GameObject go,float time)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }
    public void UpdatePlayerHealth()
    {
        if (playerHealthText == null)
        {
            playerHealthText = GameManager.Instance.playerManager.player.GetComponentInChildren<TextMeshPro>();
        }
        playerHealthText.text = GameManager.Instance.playerManager.player.Health.ToString();
    }
    public void AddAbility(Ability ability, int id)
    {
        var abilityUI = Instantiate(abilityUIPrefab, abilitiesUiHolder.transform);
        abilitiesUI.Add(abilityUI);
        abilityUI.ActivateAbility(ability, id);
    }
    public void NotEnoughActionPointsPopup()
    {
        StartCoroutine(Popup(NotEnoughActionPoints,1));
    }
    public void CanNotMovePopup()
    {
        StartCoroutine(Popup(CanNotMove, 1));
    }
    public void UpdateWave()
    {
        Debug.Log("Wave Updated");
        WaveCounterText.text = "Wave: " + GameManager.Instance.currentWave.ToString();
    }

    public void ShowWaveUpgrades(bool isTrue)
    {
        if (isTrue)
        {
            WaveUpgradesHolder.SetActive(true);
        }
        else
        {
            WaveUpgradesHolder.SetActive(false);
        }
    }
}
