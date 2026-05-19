using System.Collections;
using UnityEngine;

public class KnockbackCommand : ICommand
{
    private PlayerController playerController;
    private Vector2 enemyPosition;
    private FloatEventChannelSO onPlayerInvincible;

    public KnockbackCommand(PlayerController playerController, Vector2 enemyPosition, FloatEventChannelSO onPlayerInvincible) 
    {
        this.playerController = playerController;
        this.enemyPosition = enemyPosition;
        this.onPlayerInvincible = onPlayerInvincible;
    }

    public void Execute()
    {
        if(playerController.isKnockbacked) return; // prevent multiple knockbacks at the same time)
        if(playerController.IsInvincible) return;

        playerController.StartCoroutine(PlayerKnockbackRoutine());
    }

    private IEnumerator PlayerKnockbackRoutine()
    {
        //Stop player movemement here
        if (playerController.IsInvincible) yield break; // prevent knockback if player is invincible

        playerController.isKnockbacked = true;
        playerController.StartCoroutine(HandlePlayerInvincibility());

        float knockbackDirection = playerController.transform.position.x < enemyPosition.x ? -1f : 1f; // determine knockback direction based on player and enemy positions

        playerController.rb.linearVelocity = Vector2.zero; // stop player movement
        playerController.rb.linearVelocity = new Vector2(playerController.knockbackForceX * knockbackDirection, playerController.knockbackForceY) *
            playerController.knockbackForce;

        HitStop.Instance.Stop(0.3f);

        yield return new WaitForSeconds(playerController.KnockbackDuration);
        playerController.isKnockbacked = false;
        playerController.StartRecovery();
    }
    private IEnumerator HandlePlayerInvincibility()
    {
        playerController.isIFrameInvincible = true;
        onPlayerInvincible.RaiseEvent(playerController.InvincibilityDuration);
        yield return new WaitForSeconds(playerController.InvincibilityDuration);
        playerController.isIFrameInvincible = false;
    }
}
