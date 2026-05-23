using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Breakable
{
    //protected Animator animator;
    protected int playerLayer;
    protected Transform playerTransform;
    protected Rigidbody2D _rb;

    [Header("Chase Settings")]
    [SerializeField] protected float stopChaseRange;

    [Header("Move Settings")]
    [SerializeField] protected float moveSpeed;

    [Header("Damage")]
    [SerializeField] protected int damage;

    [Header("Knockback Force")]
    [SerializeField] protected bool canBeKnockbacked = true;
    [SerializeField] protected float knockbackForce = 5f;
    [SerializeField] protected float knockbackDuration = 0.5f;
    public bool isKnockbacked = false;

    [Header("Camera Impulse")]
    [SerializeField] protected CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /*
     *  Move knockback into here 
     */


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

    protected virtual void HandleTurn()
    {
        if(Mathf.Abs(_rb.linearVelocity.x) > 0.1f)
        {
            float angle = _rb.linearVelocity.x > 0 ? 0 : 180f;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
