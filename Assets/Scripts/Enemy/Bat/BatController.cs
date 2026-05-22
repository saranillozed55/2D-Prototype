using UnityEngine;

public class BatController : Enemy
{
    /*
     * Enemy will be on the ceiling and will attack the player when it gets in range
     */

    //RigidBody
    public Rigidbody2D _batRb;

    [Header("Referneces")]
    [SerializeField] private BoxCollider2D _physicsCollider;
    [SerializeField] private BoxCollider2D _hurtCollider;
    
    [Header("Death Settings")]
    [SerializeField] private float deathTimer = 3f;

    [Header("Idle Settings")]
    [SerializeField] private float _idleDetectRange = 10f;

    [Header("Chase Settigns")]
    [SerializeField] private float _chaseRange = 15f;

    [Header("Gizmos")]
    [SerializeField] private bool _showLineToPlayer = false;
    [SerializeField] private bool _showIdleRange = false;
    [SerializeField] private bool _showChaseRange = false;

    //move this to enemy if needed
    [Header("Raycast")]
    [SerializeField] private LayerMask obstacleLayers;
    
    //IState Getters
    public Transform PlayerTransform => playerTransform;
    public float ChaseRange => _chaseRange;
    public LayerMask ObstacleLayers => obstacleLayers;
    public float IdleDetectRange => _idleDetectRange;
    
    private StateMachine _stateMachine;
    private BatIdleState _batIdleState;
    private BatChaseState _batChaseState;

    private void Start()
    {
        _physicsCollider = GetComponent<BoxCollider2D>();
        _hurtCollider = GetComponentInChildren<BoxCollider2D>();

        _stateMachine = new StateMachine();
        _batIdleState = new BatIdleState(this, _stateMachine);
        _batChaseState = new BatChaseState(this, _stateMachine);

        if (playerTransform == null) return;
        CheckIsDead();
        _stateMachine.Initialize(_batIdleState);
    }

    private void Update()
    {
        CheckIsDead();
    }
    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    private void OnDrawGizmosSelected()
    {
        if(_showIdleRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _idleDetectRange);
        }
        if(_showLineToPlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }
        if(_showChaseRange)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseRange);
        }
    }
}
