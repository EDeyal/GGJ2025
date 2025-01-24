using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUiHandler : MonoBehaviour
{
    public int abilityID = -1;
    [SerializeField] GameObject holder;
    public bool isActivated;
    [SerializeField] Image abilityImage;
    [SerializeField] Image cooldownImage;
    [SerializeField] TextMeshProUGUI cooldownAmountText;
    [SerializeField] TextMeshProUGUI costText;
    public void ActivateAbility(Ability ability,int id)
    {
        holder.SetActive(true);
        isActivated = true;
        abilityImage.sprite = ability.abilitySprite;
        abilityID = id;
    }
    public void UpdateCooldown(Ability ability)
    {
        if (ability.CurrentCooldown != 0)
        {
            cooldownImage.gameObject.SetActive(true);
            cooldownAmountText.text = ability.CurrentCooldown.ToString();
        }
        else
        { 
            cooldownImage.gameObject.SetActive(false);
        }
    }
    public void SelectAbility()
    {
        GameManager.Instance.playerManager.ChooseAbility(abilityID);
    }
}
