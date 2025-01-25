using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int maxActionPoints;
    private int _actionPoints = 0;
    private int _health = 0;
    [SerializeField] private int _healthRegeneration = 0;
    public int HealthRegeneration => _healthRegeneration;
    public int Health => _health;
    [SerializeField] private int _maxHealth = 0;
    public bool isPlayerDead;
    public List<Ability> abilities;

    bool _playerIsMoving;

    [SerializeField] Animator _knifeAnimator;
    public Animator KnifeAnimator => _knifeAnimator;
    [SerializeField] ParticleSystem _bloodEffect;

    void Awake()
    {
        abilities = new List<Ability>();
        RefreshActionPoints();
        _health = _maxHealth;
    }
    public int GetLatestAbilityID()
    {
        return abilities.Count - 1;
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
           var bloodEffect = Instantiate(_bloodEffect, transform);
            Destroy(bloodEffect, 2f);
        }
        if (_health + amount > _maxHealth)
            _health = _maxHealth;
        else if (_health + amount <= 0)
        {
            _health = 0;
            GameManager.Instance.GameOver();
        }
        else
        {
            _health += amount;
        }
        GameManager.Instance.uiManager.UpdatePlayerHealth();
    }

    public int ActionPoints {
        get
        {
            return _actionPoints;
        }
        set
        {
            _actionPoints = value;
            //Debug.Log("Action Points changed" + ActionPoints);
        }
        }


    void Update()
    {
        if (!isPlayerDead)
        {
            if (GameManager.Instance.IsPlayerTurn)
            {
                CheckforMovement();
            }
        }
    }
    void CheckforMovement()
    {
        if (_playerIsMoving)
            return;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (GameManager.Instance.playerManager.IsAbilitySelected)
            {
                GameManager.Instance.uiManager.CanNotMovePopup();
                return;
            }
            TryMove(new Vector3(0, 0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (GameManager.Instance.playerManager.IsAbilitySelected)
            {
                GameManager.Instance.uiManager.CanNotMovePopup();
                return;
            }
            TryMove(new Vector3(0, 0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (GameManager.Instance.playerManager.IsAbilitySelected)
            {
                GameManager.Instance.uiManager.CanNotMovePopup();
                return;
            }
            TryMove(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (GameManager.Instance.playerManager.IsAbilitySelected)
            {
                GameManager.Instance.uiManager.CanNotMovePopup();
                return;
            }
            TryMove(new Vector3(1, 0, 0));
        }
    }
    void TryMove(Vector3 movement)
    {
        if (GameManager.Instance.gridHandler.CheckIsNodeOccupied((int)(transform.position.x + movement.x), (int)(transform.position.z + movement.z)))
        {
            //Debug.LogWarning("Tile " + (transform.position.x + movement.x) + "," + (transform.position.z + movement.z) + " is already occupied or a wall");
        }
        else if (ActionPoints <= 0)
        {
            //Debug.LogWarning("Out Of ActionPoints");
        }
        else
        {
            Move(movement);
        }
    }
    private void Move(Vector3 movement)
    {
        _playerIsMoving = true;
        var gm = GameManager.Instance;

        // Clear previous position
        gm.gridHandler.SetIsNodeOccupied((int)transform.position.x, (int)transform.position.z, false);

        // Calculate target position
        Vector3 targetPosition = transform.position + movement;

        // Start coroutine to move and rotate smoothly
        StartCoroutine(SmoothMoveAndRotate(targetPosition, movement, 0.5f)); // 0.5f = movement duration

        // Set new position occupied after moving
        gm.gridHandler.SetIsNodeOccupied((int)targetPosition.x, (int)targetPosition.z, true);

        // Reduce action points by 1
        ActionPoints -= 1;
        gm.uiManager.UpdatePlayerActionText(ActionPoints);

        if (CheckActionPointsReachedZero())
        {
            Debug.Log("Player eneded his turn");
            //player auto turn end
            GameManager.Instance.SwapTurn();
        }
    }
    private IEnumerator SmoothMoveAndRotate(Vector3 targetPosition, Vector3 movement, float duration)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        // Calculate target rotation based on movement direction
        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate position over time
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // Interpolate rotation over time
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position and rotation are set accurately
        Debug.Log("Playermoved");
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        _playerIsMoving = false;
    }
    public void IncreaseActionPoints(int amount)
    {
        maxActionPoints += amount;
        ActionPoints += amount;
        GameManager.Instance.uiManager.UpdatePlayerActionText(ActionPoints);
    }
    public void RefreshActionPoints()
    {
        ActionPoints = maxActionPoints;
        //Debug.Log("refreshed action points");
        GameManager.Instance.uiManager.UpdatePlayerActionText(ActionPoints);
    }
    public void ResetActionPoints()
    {
        ActionPoints = 0;
    }
    public bool CheckActionPointsReachedZero()
    {
        if (ActionPoints == 0)
            return true;
        return false;
    }
    public void ReduceAbilitiesCooldown()
    {
        foreach (var ability in abilities)
        {
            ability.ReduceCooldown();
        }
    }
    public bool ReduceActionPoints(int amount)
    {
        if (ActionPoints >= amount)
        {
            _actionPoints -= amount;
            GameManager.Instance.uiManager.UpdatePlayerActionText(_actionPoints);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void RegenerateHealth()
    {
        ChangeHealth(_healthRegeneration);
    }
    public void IncreaseHealthRegeneration(int amount)
    {
        _healthRegeneration += amount;
        GameManager.Instance.uiManager.UpdatePlayerHealth();
    }
    public void IncreaseMaxHealth(int amount)
    {
        _maxHealth += amount;
        ChangeHealth(amount);
    }
}
