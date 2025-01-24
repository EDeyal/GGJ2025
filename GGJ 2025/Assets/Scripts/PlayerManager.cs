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
            GameManager.Instance.enemyManager.HitEnemy(enemyPos, SelectedAbility.damage);
            UnselectAbility();
        }
        else
        {
            GameManager.Instance.uiManager.NotEnoughActionPointsPopup();
            //UI Manager error for not having enough points
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
