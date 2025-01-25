using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    Player _player;
    Ability SelectedAbility;
    bool isAbilitySelected;
    public bool IsAbilitySelected => isAbilitySelected;
    [SerializeField] Ability _basicAbility;
    Ability ability2;
    public Player player
    {
        get { return _player; }
    }
    public void UnlockSecondAbility()
    {
        AddAbility(ability2);
    }
    public void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        if (GameManager.Instance.gridHandler.CheckIsNodeOccupied(0, 0) == false)
        {
            GameManager.Instance.gridHandler.SetIsNodeOccupied(0, 0, true);
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
        GameManager.Instance.gridHandler.ShowAbilityRange((int)player.transform.position.x, (int)player.transform.position.z, ability.abilityRange);
    }


    public void ExecuteAbility(Vector2 enemyPos)
    {
        //Debug.Log("ExecuteAbility");
        if (player.ReduceActionPoints(SelectedAbility.abilityCost))
        {
            //Activate cooldowns for the ability
            SelectedAbility.ActivateAbilityCooldown();
            //wait for attack animation before continuing
            StartCoroutine(ExecuteAttackAnimation(enemyPos, SelectedAbility));

        }
        else
        {
            GameManager.Instance.uiManager.NotEnoughActionPointsPopup();
            //UI Manager error for not having enough points
        }
    }
    private IEnumerator ExecuteAttackAnimation(Vector2 enemyPos, Ability ability)
    {
        // Calculate direction towards the enemy (on X-Z plane)
        Vector2 direction = (enemyPos - new Vector2(player.transform.position.x, player.transform.position.z)).normalized;
        Vector3 directionToEnemy = new Vector3(direction.x,0,direction.y);

        // Create a target rotation towards the enemy
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        // Debug: Check direction and target rotation
        Debug.Log("Direction to Enemy: " + directionToEnemy);
        Debug.Log("Target Rotation: " + targetRotation.eulerAngles);

        // Only rotate if the player is not already facing the target
        while (Quaternion.Angle(player.transform.rotation, targetRotation) > 1f)
        {
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Keep the player's rotation constrained on the Y-axis (to prevent flipping)
            player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);

            // Debug: Log the angle to check if it is reducing
            Debug.Log("Current Rotation Angle: " + Quaternion.Angle(player.transform.rotation, targetRotation));

            yield return null;
        }

        Debug.Log("Rotation Complete");

        // Start the attack animation (trigger the animation)
        if (ability.abilityID == 0)
        {
            player.KnifeAnimator.SetTrigger("Attack");
        }
        player.GetComponent<AudioSource>().Play();

        // Wait for the animation to complete (you can adjust the duration or wait for a specific animation event)
        yield return new WaitForSeconds(1);

        // After the animation, hit the enemy
        GameManager.Instance.enemyManager.HitEnemy(enemyPos, ability.damage);


        UnselectAbility();
        if (player.CheckActionPointsReachedZero())
        {
            Debug.Log("Player ended his turn");
            // Player auto turn end
            GameManager.Instance.SwapTurn();
        }
    }
    void UnselectAbility()
    {
        isAbilitySelected=false;
        //Debug.Log("UnselectAbility");
        SelectedAbility = null;
        GameManager.Instance.gridHandler.UnshowAbilityRange();

    }
    private void Update()
    {
        if (isAbilitySelected)
        {

            if (Input.GetMouseButtonDown(0))
            {
                if(HitTarget(out Vector2 gridLocation))
                {
                    ExecuteAbility(gridLocation);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                UnselectAbility();
            }

        }
    }

    bool HitTarget(out Vector2 gridLocation)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from the mouse position
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("Floor");

        // Perform the raycast and filter by layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("Hit object: " + hit.collider.name + " on layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            // Get the desired component from the hit object
            GridNode tile = hit.collider.GetComponent<GridNode>();
            if (tile != null)
            {
                //get targeted tile
                if (tile.IsHighlighted)
                {
                    if (tile.isOccupied)
                    {
                        Debug.Log("Tile component found: " + tile.name);
                        gridLocation = new Vector2(tile.xPos,tile.yPos);
                        return true;
                    }
                    else
                    {
                        Debug.Log("Tile is not occupied");
                    }
                }
                else
                {
                    Debug.Log("tile is not Highlighted");
                }
            }
            else
            {
                Debug.Log("Object is on the correct layer but does not have a TileComponent.");
            }
        }
        else
        {
            Debug.Log("Raycast didn't hit any object on the specified layer.");
        }
        gridLocation = new Vector2( -1,-1);
        return false;
    }
}
