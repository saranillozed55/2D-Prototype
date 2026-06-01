using Unity.VisualScripting;
using UnityEngine;

public class NecromancerBoss : BossBase
{
    private StateMachine _stateMachine;

    //Animation references
    public static readonly int IsIdleHash = Animator.StringToHash("IsIdle");
    public static readonly int IsWalkHash = Animator.StringToHash("IsWalking");
    public static readonly int IsAttack1Hash = Animator.StringToHash("IsAttack1");
    public static readonly int IsAttack2Hash = Animator.StringToHash("IsAttack2");
    public static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    [Header("Attack 1 Settings")]
    [SerializeField] protected float _attack1Range = 1.5f;
    [SerializeField] protected int _attack1Damage = 5;

    [Header("Attack 2 Settings")]
    [SerializeField] protected float _attack2Range = 2f;
    [SerializeField] protected int _attack2Damage = 10;

    [Header("Listener to Event Channels")]
    [SerializeField] private VoidEventChannelSO _playerWalkedIntoBoss;

    //base state 
    private NecromancerWaitingState _necroWaitingState;

    [Header("Player Detection")]
    [SerializeField] private LayerMask _playerLayerMask;

    [Header("Attack Triggers")]
    [SerializeField] private BoxCollider2D _attack1Trigger;
    [SerializeField] private BoxCollider2D _attack2Trigger;

    private bool _isWaiting = true;
    private float _idleTimer;
    private float _idleTime = 2f;

    //use for cone if wanted
    private float _viewAngle = 45f;

    //Used to determine which attack state previously so we alternate between attacks
    private EnemyState<NecromancerBoss> _previousAttackState;
    public EnemyState<NecromancerBoss> PreviousAttackState
    {
        get => _previousAttackState;
        set
        {
            if(_previousAttackState != value)
            {
                _previousAttackState = value;
            }
        }
    } 

    private void Start()
    {
        _stateMachine = new StateMachine();
        _necroWaitingState = new NecromancerWaitingState(this, _stateMachine);
        _stateMachine.Initialize(_necroWaitingState);
    }

    private void OnEnable()
    {
        _playerWalkedIntoBoss.OnEventRaised += HandlePlayerWalkedIntoBoss;
        OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _playerWalkedIntoBoss.OnEventRaised -= HandlePlayerWalkedIntoBoss;
        OnDeath -= HandleDeath;
    }

    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    private void Update()
    {
        if (Time.time >= _idleTimer && _stateMachine.CurrentState == _necroWaitingState && _isWaiting == false)
        {
            _stateMachine.TransitionTo(new NecromancerWalkState(this, _stateMachine));
            _idleTimer = 0f; // reset the timer after transitioning to walk state
        }
    }

    public void OnContactDamTrigger(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerData data = collision.GetComponent<PlayerData>();

            if(data != null)
            {
                data.LoseHealth(_contactDamage, transform.position);
            }
        }
    }

    public void OnAttackHit(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player hit by necromancer attack!");
            PlayerData data = collision.GetComponent<PlayerData>();

            //have to add better way of doing this if we add more attacks but this works for now
            int damage = _previousAttackState is NecromancerAttack1State ? _attack1Damage : _attack2Damage;
            
            if (data != null)
            {
                data.LoseHealth(damage, transform.position);
            }
        }
    }


    private void HandleDeath() 
    {
        _stateMachine.TransitionTo(new NecromancerDeathState(this, _stateMachine));
    }

    private void HandlePlayerWalkedIntoBoss()
    {
        Debug.Log("Player walked into the boss room, starting the fight!");
        _isWaiting = false;
        //Want the player to be able to react to the boss before it start chasing
        _idleTimer = Time.time + _idleTime;
    }

    public void OnAttackAnimationComplete()
    {
        // This method can be called from an animation event at the end of the attack animation
        // You can use this to reset any necessary variables or transition back to another state
        Debug.Log("Attack animation completed!");
        _stateMachine.TransitionTo(new NecromancerWalkState(this, _stateMachine));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * _attack1Range);
    }

    public void SetPreviousAttackState(EnemyState<NecromancerBoss> attackState)
    {
        if (_previousAttackState != attackState)
        {
            _previousAttackState = attackState;
        }
    }
    public void StopBossMovement()
    {
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
    }

    public void RaiseAnimationColliders(int attackIndex)
    {
        switch (attackIndex)
        {
            case 1:
                _attack1Trigger.enabled = true;
                break;
            case 2:
                _attack2Trigger.enabled = true;
                break;
        }
    }
    public void DisableAnimationColliders(int attackIndex)
    {
        switch (attackIndex)
        {
            case 1:
                _attack1Trigger.enabled = false;
                break;
            case 2:
                _attack2Trigger.enabled = false;
                break;
        }
    }

    public LayerMask PlayerLayerMask => _playerLayerMask;
    public float Attack1Range => _attack1Range;
    public float Attack1Damage => _attack1Damage; // don't know where to use this yet
}
