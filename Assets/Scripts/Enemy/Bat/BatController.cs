using UnityEngine;

public class BatController : Enemy
{
    [Header("References")]
    [SerializeField] private BoxCollider2D _physicsCollider;
    [SerializeField] private BoxCollider2D _hurtCollider;
    public Rigidbody2D _batRb;
    public Animator _animator;

    [Header("Idle Settings")]
    [SerializeField] private float _idleDetectRange = 10f;

    [Header("Chase/Flying Settigns")]
    [SerializeField] private float _chaseRange = 15f;
    [SerializeField] private float _floatingAmplitude = 1.5f; // how high we bob and up and down
    [SerializeField] private float _floatFrequency = 5f; // how fast we bob up and down

    [Header("Gizmos")]
    [SerializeField] private bool _showLineToPlayer = false;
    [SerializeField] private bool _showIdleRange = false;
    [SerializeField] private bool _showChaseRange = false;

    //move this to enemy if needed
    [Header("Raycast")]
    [SerializeField] private LayerMask obstacleLayers;

    //IState Getters
    public float MoveSpeed => _moveSpeed;
    public float FloatingAmplitude => _floatingAmplitude;
    public float FloatingFrequency => _floatFrequency;
    public Transform PlayerTransform => _playerTransform;
    public float ChaseRange => _chaseRange;
    public LayerMask ObstacleLayers => obstacleLayers;
    public float IdleDetectRange => _idleDetectRange;
    public float DeathTimer => _deathTimer;
    
    private BatIdleState _batIdleState;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _batRb = GetComponent<Rigidbody2D>();
        _physicsCollider = GetComponent<BoxCollider2D>();
        _hurtCollider = GetComponentInChildren<BoxCollider2D>();

        _stateMachine = new StateMachine();
        _batIdleState = new BatIdleState(this, _stateMachine);

        _stateMachine.Initialize(_batIdleState);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (isKnockbacked || isDead) return;
        _stateMachine.Update();
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if(_showIdleRange) 
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(transform.position, _idleDetectRange);
    //    }
    //    if(_showLineToPlayer && playerTransform != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(transform.position, playerTransform.position);
    //    }
    //    if(_showChaseRange)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(transform.position, _chaseRange);
    //    }
    //}
}
