using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Breakable
{
    //protected Animator animator;
    protected int playerLayer;
    protected Transform playerTransform;

    [Header("Chase Settings")]
    [SerializeField] protected float stopChaseRange;

    [Header("Move Settings")]
    [SerializeField] protected float moveSpeed;

    [Header("Damage")]
    [SerializeField] protected int damage;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.layer == playerLayer)
        {
            var playerData = collision.gameObject.GetComponentInParent<PlayerData>();
            var playerController = collision.gameObject.GetComponentInParent<PlayerController>();
            if (!playerController.IsInvincible)
            {
                playerData.LoseHealth(damage,transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);
    }
}
