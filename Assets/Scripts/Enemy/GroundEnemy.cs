using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header("WayPoints/Patrol")]
    [SerializeField] protected PatrolPath _enemyPath;
    [SerializeField] protected float _edgePauseDuration = 2f;
    [SerializeField] protected float _enemySpottedPause = 1f;
    public int _currentWayPointIndex = 0;
    public bool _atPatrolEdge = false;

    [Header("Ledge Detection")]
    [SerializeField] protected float _groundCheckRadius = 0.2f;
    [SerializeField] protected LayerMask _groundLayerMask;
    [SerializeField] protected Vector2 _detectDistance = new Vector2(0.05f, 0.0f);
    [SerializeField] protected float _yDetectDistance = 0.1f;
    protected float _groundedCheckDistance = 0.05f;
    protected bool _isGrounded = false;
    protected bool _fEdgeDetected;
    protected Vector2 leftEdge;
    protected Vector2 rightEdge;

    [Header("Wall Detection")]
    [SerializeField] protected float _rayLength = 2f;
    [SerializeField] protected LayerMask _obstacleLayerMask;
    protected bool _wallDetected;

    [Header("Fall Speed")]
    [SerializeField] protected float _maxFallSpeed = 15f;

    [Header("Cone vision")]
    [SerializeField] protected float viewDistance = 10f;
    [SerializeField] protected float viewAngle = 45f;

    [Header("Edge Detection Gizmos")]
    [SerializeField] private bool _showGizmos;
    private Vector2 _originTestGizmo;

    [Header("Detection")]
    [SerializeField] private float _stopChaseRange = 5f;
    [SerializeField] private float loseAggroRange = 10f;

    public event Action OnSpottedPlayer;

    protected virtual void Update()
    {
        _isGrounded = CheckGrounded();
        HandleTurn();
        ClampFallSpeed();
        EdgeDetection();
        WallDetection();
    }

    public virtual bool IsTargetInCone()
    {
        Vector3 dirToTarget = (_playerTransform.position - transform.position).normalized;

        //check distance 
        float dist = Vector3.Distance(transform.position, _playerTransform.position);
        if (dist > viewDistance) return false;

        //return cosine of the angle between vectors
        float dotProduct = Vector3.Dot(transform.right, dirToTarget);

        //compare aginst the threshold, divide 2 because viewangle is the total width but we measure from the center
        float angleThreshold = Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad);

        if (dotProduct >= angleThreshold)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, viewDistance, _obstacleLayerMask | (1 << _playerLayer));
            if (hit.collider != null && hit.transform == _playerTransform)
            {
                if (hit.transform == _playerTransform) return true;
            }
            return false; // wall or nothing was hit - not visible
        }
        return false;
    }

    public void NotifySpottedPlayer()
    {
        OnSpottedPlayer?.Invoke();
    }

    private void ClampFallSpeed()
    {
        if (_rb.linearVelocity.y < -_maxFallSpeed)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _maxFallSpeed);
        }
    }

    private void EdgeDetection()
    {
        if (!_isGrounded)
        {
            _fEdgeDetected = false;
            return;
        }

        if (_isGrounded)
        {
            //bottom left corner
            Vector2 bottomLeft = new Vector2(_boxPhysicsCollider.bounds.min.x, _boxPhysicsCollider.bounds.min.y);

            //bottom right corner
            Vector2 bottomRight = new Vector2(_boxPhysicsCollider.bounds.max.x, _boxPhysicsCollider.bounds.min.y);

            leftEdge = bottomLeft - _detectDistance;
            rightEdge = bottomRight + _detectDistance;

            if (_isFacingRight)
            {
                RaycastHit2D hit = Physics2D.Raycast(rightEdge, Vector2.down, _yDetectDistance, _groundLayerMask);
                _fEdgeDetected = !hit; // true if no ground, false if gorund
                if (_fEdgeDetected) Debug.Log("Right side of enemy is ledge");
            }
            else if (!_isFacingRight)
            {
                RaycastHit2D hit = Physics2D.Raycast(leftEdge, Vector2.down, _yDetectDistance, _groundLayerMask);
                _fEdgeDetected = !hit; // true if no ground, false if gorund
                if (_fEdgeDetected) Debug.Log("Left side of enemy is ledge");

            }
            else
            {
                _fEdgeDetected = false;
            }
        }
    }

    private void WallDetection()
    {
        Vector2 wallCheckDirection = _isFacingRight ? Vector2.right : Vector2.left;

        float originX = _isFacingRight ? _boxPhysicsCollider.bounds.max.x : _boxPhysicsCollider.bounds.min.x;
        Vector2 origin = new Vector2(originX, _boxPhysicsCollider.bounds.center.y);

        _originTestGizmo = origin;

        RaycastHit2D hit = Physics2D.Raycast(origin, wallCheckDirection, _rayLength, _obstacleLayerMask);
        _wallDetected = hit.collider != null;
    }

    private bool CheckGrounded()
    {
        Vector2 origin = new Vector2(_boxPhysicsCollider.bounds.center.x, _boxPhysicsCollider.bounds.min.y);

        RaycastHit2D hit = Physics2D.CircleCast(origin, _groundCheckRadius, Vector2.down, _groundedCheckDistance, _groundLayerMask);
        return hit.collider != null;
    }

    protected void OnDrawGizmosSelected()
    {
        if (_showGizmos == false) return;

        Vector2 wallCheckDirection = _isFacingRight ? Vector2.right : Vector2.left;

        Gizmos.color = Color.brown;
        Gizmos.DrawSphere(leftEdge, 0.1f);

        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawSphere(rightEdge, 0.1f);

        Gizmos.color = Color.beige;
        Gizmos.DrawRay(_originTestGizmo, wallCheckDirection);

        Gizmos.color = Color.chocolate;
        Gizmos.DrawSphere(_lastSeenPosition, 0.5f);
    }

    //getters for states to access variables
    public PatrolPath EnemyPath => _enemyPath;
    public float MoveSpeed => _moveSpeed;
    public Transform PlayerLocation => _playerTransform;
    public float ViewDistance => viewDistance;
    public float ViewAngle => viewAngle;
    public float StopChaseRange => _stopChaseRange;
    public float LoseAggroRange => loseAggroRange;
    public float DeathTimer => _deathTimer;
    public float KnockbackForce => knockbackForce;
    public float KnockBackDuration => knockbackDuration;
    public bool EdgeFrontDetected => _fEdgeDetected;
    public bool WallDetected => _wallDetected;
    public int PlayerLayer => _playerLayer;
    public Vector2 LastSeenPosition => _lastSeenPosition;
    public bool HasLastSeenPosition => _hasLastSeenPosition;
    public float EdgePauseDuration => _edgePauseDuration;
    public float EnemySpottedPause => _enemySpottedPause;
}
