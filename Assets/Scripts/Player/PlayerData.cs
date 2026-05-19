using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Broadcast on Event Channels")]
    [SerializeField] private VoidEventChannelSO _onPlayerDeath;
    [SerializeField] private VoidEventChannelSO _onPlayerRespawn;
    [SerializeField] private FloatEventChannelSO _onPlayerInvincibility;

    [Header("Listener on Event Channels")]
    [SerializeField] private HazardEventChannel _hazardChannel;
    [SerializeField] private BoolEventChannel_SO _onPlayerInvincibleCheat;
    [SerializeField] private BoolEventChannel_SO _onPlayerKill;
    [SerializeField] private VoidEventChannelSO _onPlayerInfiniteHealth;

    [Header("Data")]
    [SerializeField] private int health;
    [SerializeField] private bool isDead;
    [SerializeField] private float respawnTime = 3f;

    [Header("Player SO")]
    [SerializeField] private PlayerDataSO _playerDataSO;

    [SerializeField] private PlayerInputHandler inputHandler;


    private PlayerController playerController;

    public int GetCurrentHealth => health;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        health = _playerDataSO.health;
        isDead = false;
        respawnTime = _playerDataSO.playerRespawnTime;
    }

    private void OnEnable()
    {
        _hazardChannel.OnHazardTriggered += HandleHazardDamage;
        _onPlayerKill.OnEventRaised += CheatKillPlayer;
        _onPlayerInvincibleCheat.OnEventRaised += CheatPlayerInvincible;
        _onPlayerInfiniteHealth.OnEventRaised += CheatInfiniteHealth;
    }

    private void OnDisable()
    {
        _hazardChannel.OnHazardTriggered -= HandleHazardDamage;
        _onPlayerKill.OnEventRaised -= CheatKillPlayer;
        _onPlayerInvincibleCheat.OnEventRaised -= CheatPlayerInvincible;
        _onPlayerInfiniteHealth.OnEventRaised -= CheatInfiniteHealth;

    }

    private void Update()
    {
        CheckIsDead();
    }

    private void CheckIsDead()
    {
        if(health <= 0 && !isDead)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player has Died");
        if (!isDead)
        {
            StartCoroutine(DeathCoroutine());
        }
    }

    public void LoseHealth(int health,Vector2 enemyPosition)
    {
        if (isDead || playerController.IsInvincible) return;
        this.health -= health;
        Debug.Log("Player Health: " + this.health);
        ICommand command = new KnockbackCommand(playerController, enemyPosition, _onPlayerInvincibility);
        command.Execute();  
    }

    // check if player is dead. allows for other scripts to check.
    public bool GetDeadStatement()
    {
        CheckIsDead();
        return isDead;
    }

    public void SetHazardRespawnData(int health)
    {
        if(health > 0)
        {
            this.health = health;
            isDead = false;
        }
    }

    private void HandleHazardDamage(GameObject source, GameObject target)
    {
        health -= 5;
        Debug.Log("Updated Health: " + health + " and was hit by Hazard");
    }
    private IEnumerator DeathCoroutine()
    {
        isDead = true;

        //set state death so player can do anything
        _onPlayerDeath.RaiseEvent();

        //ADD: send event for animations
        yield return new WaitForSeconds(respawnTime);

        isDead = false;
        ResetPlayer();

        _onPlayerRespawn.RaiseEvent();
    }

    private void ResetPlayer()
    {
        // go back to save/safe points
        health = _playerDataSO.health;
    }

    private void CheatKillPlayer(bool param)
    {
        if(health > 0 || !isDead)
        {
            health = 0;
            isDead = true;
        }
    }

    private void CheatPlayerInvincible(bool param)
    {
        playerController.isCheatInvincible = true;
    }

    private void CheatInfiniteHealth()
    {
        if(!isDead)
        {
            health = int.MaxValue;
        }
    }

}
