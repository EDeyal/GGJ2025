using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerActionPoints;
    [SerializeField] TextMeshPro playerHealthText;
    [SerializeField] GameObject playerTurn;
    [SerializeField] GameObject enemyTurn;
    [SerializeField] GameObject gameOver;
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
            StartCoroutine(SwapTurn(go));
    }

    IEnumerator SwapTurn(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(0.5f);
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
}
