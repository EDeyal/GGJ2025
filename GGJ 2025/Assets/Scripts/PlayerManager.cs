using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    Player _player;
    Ability SelectedAbility;
    bool isAbilitySelected;
    [SerializeField] Ability _basicAbility;
    Ability ability2;
    Ability ability3;
    public Player player { 
        get { return _player; }}

    public void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab, new Vector3(0,1,0),Quaternion.identity);
        if (GameManager.Instance.gridHandler.CheckIsNodeOccupied(0,0) == false)
        {
            GameManager.Instance.gridHandler.SetIsNodeOccupied(0,0, true);
        }
        AddAbility(_basicAbility);
    }
    public void StartPlayerTurn()
    {
        player.RefreshActionPoints();
        player.ReduceAbilitiesCooldown();
    }
    public void AddAbility(Ability ability)
    {
        _player.abilities.Add(ability);
        GameManager.Instance.uiManager.AddAbility(ability, player.GetLatestAbilityID());
    }
    public void ChooseAbility(int abilityID)
    {
        var ability = player.abilities[abilityID];

        isAbilitySelected = true;
        SelectedAbility = ability;

        Debug.Log("AbilitySelected");

        //show ability range tiles
        GameManager.Instance.gridHandler.ShowAbilityRange((int)player.transform.position.x,(int)player.transform.position.z, ability.abilityRange);
    }


    public void ExecuteAbility()
    {
        //get targeted tile
        //if occupied get the occuping target and hit it
        //Activate cooldowns for the ability
        //ifNotOccupied do nothing.
    }
    void UnselectAbility()
    {
        SelectedAbility = null;
        GameManager.Instance.gridHandler.UnshowAbilityRange();

    }
    private void Update()
    {
        if (isAbilitySelected)
        {

            if (Input.GetMouseButtonDown(0))
            {
                ExecuteAbility();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                UnselectAbility();
            }

        }
    }
}
