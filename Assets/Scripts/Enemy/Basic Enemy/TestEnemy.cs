using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TestEnemy : Enemy
{
    public Rigidbody2D _enemyRb;
    public BoxCollider2D PhysicsCollider { get; private set; }
    public BoxCollider2D HurtBox { get; private set; }

    //Move waypoints to enemy and then just check if the enemypath is not null
    [Header("Waypoints")]
    public PatrolPath EnemyPath;

    [Header("Cone vision")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 45f;

    [Header("Death Settings")]
    [SerializeField] private float deathTimer = 2f;
    

    [Header("Hurt Settings")]
    [SerializeField] private bool isHurt;
   
    [Header("Detection")]
    [SerializeField] private float spotRange = 5f;
    [SerializeField] private float loseAggroRange = 10f;

    //Events to visuals and other scripts
    public event Action OnSpottedPlayer; // move this

    //properties for states to access private variables
    public float MoveSpeed => moveSpeed;
    public Transform PlayerLocation => playerTransform;
    public float ViewDistance => viewDistance;
    public float ViewAngle => viewAngle;
    public float StopChaseRange => spotRange;
    public float LoseAggroRange => loseAggroRange;
    public float DeathTimer => deathTimer;
    public float KnockbackForce => knockbackForce;
    public float KnockBackDuration => knockbackDuration;

    private StateMachine _stateMachine;
    private PatrolState _patrolState;
    private ChaseState _chaseState;
    private DeathState _deathState;

    private void Start()
    {
        _enemyRb = _rb;
        PhysicsCollider = GetComponent<BoxCollider2D>();
        HurtBox = GetComponentInChildren<BoxCollider2D>();

        _stateMachine = new StateMachine();
        _patrolState = new PatrolState(this, _stateMachine);
        _chaseState = new ChaseState(this, _stateMachine);
        _deathState = new DeathState(this, _stateMachine);
        
        _stateMachine.Initialize(_patrolState);
        isHurt = false;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        CheckIsDead();

        if (isKnockbacked) return;
        HandleTurn();
    }

    private void FixedUpdate()
    {
        if (isKnockbacked) return; // prevent state updates during knockback
        _stateMachine.Update();
    }

    protected override void Dead()
    {
        base.Dead();
        _enemyRb.linearVelocity = Vector2.zero;
        _stateMachine.TransitionTo(new DeathState(this, _stateMachine));
    }

    //enemy knockback
    public override void Hurt(int damage)
    {
        base.Hurt(damage);

        _impulseSource.GenerateImpulse(Vector3.up * 0.1f);
        HitStop.Instance.Stop(0.05f);
        
        if (!isHurt)
        {
            Debug.Log("Enemy was hurt. Starting knockback.");
            StartCoroutine(KnockBackRoutine());
        }
    }

    private IEnumerator KnockBackRoutine()
    {
        isKnockbacked = true;
        isHurt = true;
        _enemyRb.linearVelocity = Vector2.zero;
        _enemyRb.linearVelocity = -transform.right * KnockbackForce;
        yield return new WaitForSeconds(0.2f);
        isKnockbacked = false;
        isHurt = false;
    }

    public bool IsTargetInCone()
    {
        Vector3 dirToTarget = (playerTransform.position - transform.position).normalized;

        //check distance 
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist > viewDistance) return false;

        //return cosine of the angle between vectors
        float dotProduct = Vector3.Dot(transform.right, dirToTarget);

        //compare aginst the threshold, divide 2 because viewangle is the total width but we measure from the center
        float angleThreshold = Mathf.Cos(viewAngle * 0.5f *Mathf.Deg2Rad);

        if(dotProduct >= angleThreshold)
        {
            if(Physics.Raycast(transform.position, dirToTarget, out RaycastHit hit, viewDistance))
            {
                if (hit.transform == playerTransform) return true;
            }
        }
        return dotProduct >= angleThreshold;
    }

    public void NotifySpottedPlayer()
    {
        OnSpottedPlayer?.Invoke();
    }


    //Refactor so that any state knows about the player always faces them, and only patrol uses velocity-based facing
    //Refactor: Add this to Enemy instead because all enemies will have to turn to the enemy.
    protected override void HandleTurn()
    {
        //if alerting or chasing, face the player directly
        if (_stateMachine.CurrentState is ChaseState)
        {
            float directionToPlayer = playerTransform.position.x - transform.position.x;
            float angle = directionToPlayer > 0 ? 0 : 180f;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        else
        {
            base.HandleTurn();
        }
 
    }
}
