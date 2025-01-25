using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public int abilityID =0 ;
    public int abilityRange;
    public int damage;
    public int abilityCooldown;
    int _currentCooldown;
    public int CurrentCooldown => _currentCooldown;
    public int abilityCost;
    public Sprite abilitySprite;

    public bool CanUseAbility(int CurrentActionPoints)
    {
        if (CurrentActionPoints < abilityCost || _currentCooldown > 0)
            return false;
        return true;
    }
    public void ReduceCooldown()
    {
        if(_currentCooldown > 0)
            _currentCooldown--;
    }
    public void ActivateAbilityCooldown()
    {
        _currentCooldown = abilityCooldown;
    }
}
