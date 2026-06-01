using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Breakable
{
    //protected Animator animator;
    public Rigidbody2D _rb;
    protected int _playerLayer;
    protected Transform _playerTransform;
    protected BoxCollider2D _boxPhysicsCollider;
    protected bool _isFacingRight;
    protected CameraShakeSource _cameraShakeSource;
    public Animator _animator;

    private DamageFlash _damageFlash;

    public bool IsChasing { get; set; }

    [Header("Move Settings")]
    [SerializeField] protected float _moveSpeed; // don't have move speed here have it on ground enemy and make it so that air enemies have air speed instead(better naming)

    [Header("Damage")]
    [SerializeField] protected int _damage;

    [Header("Death Settings")]
    [SerializeField] protected float _deathTimer = 2f;

    [Header("Knockback Settings")]
    [SerializeField] protected bool canBeKnockbacked = true;
    [SerializeField] protected float knockbackForce = 5f;
    [SerializeField] protected float knockbackDuration = 0.5f;
    public bool isKnockbacked = false;
    protected bool isHurt = false;

    [Header("Search")]
    protected Vector2 _lastSeenPosition;
    protected bool _hasLastSeenPosition;

    [Header("Camera Impulse")]
    [SerializeField] protected CinemachineImpulseSource _impulseSource;

    //State machine reference
    protected StateMachine _stateMachine;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxPhysicsCollider = GetComponent<BoxCollider2D>();
        _playerLayer = LayerMask.NameToLayer("Player");
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _damageFlash = GetComponent<DamageFlash>();
        _cameraShakeSource = GetComponent<CameraShakeSource>();
    }
    protected virtual void Start()
    {
        _stateMachine = new StateMachine();
    }

    public override void Hurt(int damage, Vector2 hitDirection)
    {
        base.Hurt(damage, hitDirection);

        //sample impulse
        //Impulse/Camera shake here using OnHurt

        _cameraShakeSource.ShakeCamera(0.1f, Vector3.up);
        HitStop.Instance.Stop(0.05f);

        if (!isHurt && canBeKnockbacked)
        {
            //direction from player to enemy (away from hit source)
            Vector2 knockbackDir = ((Vector2)transform.position - hitDirection).normalized;
            StartCoroutine(KnockbackRoutine(knockbackDir));
        }
        _damageFlash.CallDamageFlash();
    }

    protected override void Dead()
    {
        if (isDead) return;
        base.Dead();
        _rb.linearVelocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        StartCoroutine(EnemyDeathRoutine());
    }

    protected virtual IEnumerator EnemyDeathRoutine()
    {
        yield return new WaitForSeconds(_deathTimer);
        Destroy(gameObject);
    }

    protected virtual IEnumerator KnockbackRoutine(Vector2 attackerPosition)
    {
        isKnockbacked = true;
        isHurt = true;
        _rb.linearVelocity = attackerPosition.normalized * knockbackForce;
        yield return new WaitForSeconds(0.2f);
        _rb.linearVelocity = Vector2.zero;
        isKnockbacked = false;
        isHurt = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.layer == _playerLayer)
        {
            var playerData = collision.gameObject.GetComponentInParent<PlayerData>();
            var playerController = collision.gameObject.GetComponentInParent<PlayerController>();
            if (!playerController.IsInvincible)
            {
                playerData.LoseHealth(_damage,transform.position);
            }
        }
    }

    public void UpdateLastKnowPosition(Vector2 position)
    {
        _lastSeenPosition = position;
        _hasLastSeenPosition = true;
    }

    public void ClearLastKnowPosition()
    {
        _hasLastSeenPosition = false;
    }

    protected virtual void HandleTurn()
    {
        if (isKnockbacked)
        {
            return;
        }
        if (Mathf.Abs(_rb.linearVelocity.x) > 0.1f)
        {
            float angle;
            if(_rb.linearVelocity.x > 0)
            {
                angle = 0;
                _isFacingRight = true;
            }
            else
            {
                angle = 180f;
                _isFacingRight = false;
            }

            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        else if(!IsChasing) // dont snap-face player while actively chasing
        {
            bool playerIsRight = _playerTransform.position.x > transform.position.x;
            if(playerIsRight != _isFacingRight)
            {
                _isFacingRight = playerIsRight;
                float angle = _isFacingRight ? 0 : 180f;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
    }
}
