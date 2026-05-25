using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

//[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] on static variables - force global variables bakc to their default values before the scene starts
public class PlayerController : MonoBehaviour
{
    //move this onto a different script and just access the state there PlayerStateHandler
    public PlayerState state { get; private set; }

    [Header("Listen to Event Channels")]
    [SerializeField] private VoidEventChannelSO _onPlayerDeath;
    [SerializeField] private VoidEventChannelSO _onPlayerRespawn;

    [Header("Input Handler")]
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Movemenent Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;

    [Header("Physics")]
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 100f;
    [SerializeField] private float airAcceleration = 20f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f; // amount of jump force
    [SerializeField] private float jumpCutMultiplier = 0.5f; // quick jump and long jump
    [SerializeField] private int jumpLeft; //jump and double jump
    [SerializeField] private int maxJumpLeft = 1;
    private bool hasDoubleJump = false;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f; //max allowed time for grace period
    private float coyoteTimeCounter; //counter that tracks remaining time, decreasing when not grounded, resetting when grounded

    [Header("Corner correction")]
    [SerializeField] private float cornerCorrectionDistance = 0.12f; //max pixel overlap to correct
    [SerializeField] private float cornerCorrectionHeight = 0.1f; // how far up to nudge
    [SerializeField] private float cornerCorrectionLength = 0.3f;
    [SerializeField] private float detectionWidth = 0.5f;

    [Header("Add Jump Input Buffer")]
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance;
    public bool isGrounded { get; private set; }

    [Header("Falling")]
    [SerializeField] private float fallGravityMultiplier = 3f;
    [SerializeField] private float regularGravityMultiplier = 1f;
    [SerializeField] private float apexGravityMultiplier = 0.4f;
    [SerializeField] private float apexThreshold = 1.5f; // velocity window around apex
    [SerializeField] private float minHoldTimeForApex = 0.1f;
    [SerializeField] private float maxFallSpeedClamp = 10f;
    private float jumpHoldTimer = 0f;
    private bool apexEarned = false;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.6f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashCooldownTimer;
    private bool isDashing;
    private bool canDash;
    private bool isFloating = false;

    public Rigidbody2D rb; // change to private later
    private BoxCollider2D boxCol;

    [Header("Knockback Recovery")]
    [SerializeField] private float recoveryAcceleration = 10f;
    [SerializeField] private float recoveryTime = 0.2f;
    [SerializeField] private float knockbackDuration = 0.5f;
    private float recoveryTimer;
    public bool isKnockbacked = false;
    public float knockbackForceX = 5f;
    public float knockbackForceY = 5f;
    public float knockbackForce = 5f;

    [Header("Invincibility")]
    public bool isCheatInvincible;
    public bool isIFrameInvincible;
    public bool IsInvincible => isIFrameInvincible || isCheatInvincible;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Facing Direction")]
    public bool IsFacingRight { get; private set; }

    [Header("Camera Settings")]
    [SerializeField] private GameObject cameraFollow;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    [Header("Attack Settings")]
    [SerializeField] private float attackRate = 1f;
    private float canAttack = -1f;

    [Header("Attack Collider")]
    [SerializeField] private BoxCollider2D _attackCollider2D;

    public float InvincibilityDuration => invincibilityDuration;
    public float FlashDuration => flashDuration;
    public float KnockbackDuration => knockbackDuration;
    public float CanAttack => canAttack;
    public float AttackRate => attackRate;

    public event Action AttackEvent;

    [Header("Ground Layer Mask")]
    [SerializeField] private LayerMask groundMask;

    [Header("Gizmos Bools")]
    [SerializeField] private bool groundedGizmos = false;
    [SerializeField] private bool edgeDetectionGizmos = true;
    [SerializeField] private float edgeDetectionLength = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        IsFacingRight = true;
    }

    private void Start()
    {
        Debug.Log("Collider world size: " + boxCol.bounds.size);
        CameraManager.Instance.SetPlayerRigidody(rb);
        _attackCollider2D.enabled = false;
    }

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnTryJump += QueueJump;
            inputHandler.OnJumpReleased += HandleJumpCut;
            inputHandler.OnAttack += HandleAttack;
            inputHandler.OnDash += HandleDash;
        }
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        DoubleJumpPickup.OnDoubleJumpPickup += GrantDoubleJump;
        _onPlayerDeath.OnEventRaised += HandlePlayerDeath;
        _onPlayerRespawn.OnEventRaised += HandlePlayerRespawning;
    }
    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnTryJump -= QueueJump;
            inputHandler.OnJumpReleased -= HandleJumpCut;
            inputHandler.OnAttack -= HandleAttack;
            inputHandler.OnDash -= HandleDash;
        }
        DoubleJumpPickup.OnDoubleJumpPickup -= GrantDoubleJump;
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        _onPlayerDeath.OnEventRaised -= HandlePlayerDeath;
        _onPlayerRespawn.OnEventRaised -= HandlePlayerRespawning;
        StopAllCoroutines();

    }

    private void Update()
    {
        UpdatePlayerGroundedState();
        UpdateState();
        CoyoteTime();
        if (recoveryTime > 0)
        {
            recoveryTimer -= Time.deltaTime;
        }
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        if (inputHandler.IsJumpHeld() && !isGrounded)
        {
            jumpHoldTimer += Time.deltaTime;
            if (jumpHoldTimer >= minHoldTimeForApex)
            {
                apexEarned = true;
            }
        }
    }

    void FixedUpdate()
    {
        HeadCornerCorrect();

        if (!CanControl() || isKnockbacked) return;
        FallControl();
        MovePlayer();
        TurnCheck();

        if (jumpBufferCounter > 0)
        {
            TryJump();
        }
    }

    private void HandleDash()
    {
        Dash();
    }

    private void HandleAttack()
    {
        if (Time.time > canAttack)
        {
            Debug.Log("Attacked");
            AttackEvent?.Invoke();
            canAttack = Time.time + attackRate;
        }
    }

    private bool CanControl()
    {
        return state != PlayerState.Dashing && state != PlayerState.Death && inputHandler.IsInputEnabled
            && state != PlayerState.Respawning;
    }

    #region Jumping

    public void QueueJump()
    {
        jumpBufferCounter = jumpBufferTime;
        jumpHoldTimer = 0f;
        apexEarned = false;
    }

    /*
     * Would to like to seperate this with double jump logic? because visuals will be different, so just use the isDoubleJump boolean to determine which one to use, and then in the future if we want to add more jumps like triple jump or something we can just add more booleans or an int for how many jumps we have left
     */
    private void TryJump()
    {
        bool coyoteAvailable = coyoteTimeCounter > 0f;
        bool isDoubleJump = !isGrounded && !coyoteAvailable;

        if (!coyoteAvailable && !(isDoubleJump && hasDoubleJump)) return;

        if (jumpLeft <= 0) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);


        if (!inputHandler.IsJumpHeld())
        {
            HandleJumpCut();
        }
        
        jumpLeft--;

        //prevent double jump using coyote time
        coyoteTimeCounter = 0f;
        jumpBufferCounter = 0f;
    }

    private void HandleJumpCut()
    {
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }
    #endregion

    #region Movement

    private void MovePlayer()
    {
        if (!CanControl()) return;
        Vector2 input = inputHandler.MoveValue;

        float targetSpeed = input.x * moveSpeed;
        float currentAcceleration;

        if (recoveryTimer > 0)
        {
            currentAcceleration = recoveryAcceleration;
        }

        else if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            currentAcceleration = isGrounded ? acceleration : airAcceleration;
        }
        else
        {
            currentAcceleration = deceleration;
        }

        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, currentAcceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    public void StartRecovery()
    {
        recoveryTimer = recoveryTime;
    }

    private void HeadCornerCorrect()
    {
        if (rb.linearVelocity.y <= 0) return;

        //center of the detection box (starting at the top of player)
        Vector2 boxOrigin = new Vector2(boxCol.bounds.center.x, boxCol.bounds.max.y);

        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, new Vector2(detectionWidth, 0.1f), 0f, Vector2.up, cornerCorrectionLength, groundMask);

        if (hit.collider != null)
        {
            rb.position = new Vector2(transform.position.x, hit.point.y - 0.51f);
        }
    }

    #endregion
    private bool CheckGrounded()
    {
        //bottom center of the collider
        Vector2 origin = new Vector2(boxCol.bounds.center.x, boxCol.bounds.min.y);

        float radius = 0.2f;

        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, Vector2.down, groundCheckDistance, layerMask);
        return hitRec.collider != null;
    }

    private void UpdatePlayerGroundedState()
    {
        isGrounded = CheckGrounded();
        float verticalVelocity = rb.linearVelocity.y;

        //check if velocity is near zero to ensure they have landed, don't reset jumps while dashing
        if (isGrounded && verticalVelocity < 0.1f && !isDashing)
        {
            //reset jumps when grounded
            jumpLeft = maxJumpLeft;

            //give dash back when timer is finished
            if (dashCooldownTimer <= 0) canDash = true;
        }
    }

    private void CoyoteTime()
    {
        if (isGrounded && !isDashing)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    #region Dash

    public void Dash()
    {
        if (canDash && !isDashing)
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;

        rb.linearVelocity = new Vector2((IsFacingRight ? 1 : -1) * dashSpeed, 0f);
        rb.gravityScale = 0f;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        if (!isGrounded)
        {
            jumpLeft--;
            coyoteTimeCounter = 0f;
        }

        UpdateState();
        dashCooldownTimer = dashCooldown;
    }

    #endregion

    //For "pogo" use MoveValue.y to check if we are performing a downward input.

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        if (groundedGizmos)
        {
            Collider2D gizmoCol = GetComponent<Collider2D>();
            if (gizmoCol == null) return;

            float radius = 0.2f;

            Vector2 origin = new Vector2(gizmoCol.bounds.center.x, gizmoCol.bounds.min.y);

            Vector2 endPosition = origin + Vector2.down * groundCheckDistance;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin, radius);
            Gizmos.DrawWireSphere(endPosition, radius);
            Gizmos.DrawLine(origin, endPosition);
        }

        if (edgeDetectionGizmos)
        {
            if (boxCol == null) return;

            // 1. Calculate the center and size based on your BoxCast logic
            Vector2 boxOrigin = new Vector2(boxCol.bounds.center.x, boxCol.bounds.max.y);
            Vector2 boxSize = new Vector2(detectionWidth, 0.1f);
            Vector2 direction = Vector2.up;

            // 2. Draw the "Start" box (where the detection begins)
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxOrigin, boxSize);

            // 3. Draw the "End" box (where the detection stops)
            Gizmos.color = Color.yellow;
            Vector2 endPoint = boxOrigin + (direction * edgeDetectionLength);
            Gizmos.DrawWireCube(endPoint, boxSize);

            // 4. Draw connecting lines to show the "sweep" path
            Gizmos.DrawLine(boxOrigin + new Vector2(-detectionWidth / 2, 0), endPoint + new Vector2(-detectionWidth / 2, 0));
            Gizmos.DrawLine(boxOrigin + new Vector2(detectionWidth / 2, 0), endPoint + new Vector2(detectionWidth / 2, 0));
        }
    }
    #endregion

    #region Fall Control
    private void FallControl()
    {
        float vy = rb.linearVelocity.y;

        bool atApex = Mathf.Abs(vy) < apexThreshold && !isGrounded && apexEarned;

        if (atApex)
        {
            rb.gravityScale = apexGravityMultiplier;
        }
        else if (vy < 0)
        {
            rb.gravityScale = fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = regularGravityMultiplier;
        }
    }

    private void ClampFallSpeed()
    {
        // no upward limit in upward velocity so we set it to Mathf.Infinity
        // -maxFallSpeedClamp is the max speed at which player can fall. Terminal Velocity.
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -maxFallSpeedClamp, Mathf.Infinity));
    }
    #endregion

    #region Turning
    private void TurnCheck()
    {
        if (inputHandler.MoveValue.x > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (inputHandler.MoveValue.x < 0 && IsFacingRight)
        {
            Turn();
        }
    }
    private void Turn()
    {
    if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }
    #endregion

    private void UpdateState()
    {
        if (state == PlayerState.Death)
        {
            return;
        }
        if (isDashing)
        {
            state = PlayerState.Dashing;
            return;
        }

        else if (!isGrounded && rb.linearVelocity.y > 0) state = PlayerState.Jumping;
        else if (!isGrounded && rb.linearVelocity.y < 0) state = PlayerState.Falling;
        else if (Mathf.Abs(rb.linearVelocity.x) > 0.01f) state = PlayerState.Running;
        else state = PlayerState.Idle;
    }

    public void SetState(PlayerState newState)
    {
        state = newState;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Dialogue)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    private void GrantDoubleJump()
    {
        maxJumpLeft = 2;
        hasDoubleJump = true;
    }
    private void HandlePlayerDeath()
    {
        SetState(PlayerState.Death);
        Debug.Log(state);
    }
    private void HandlePlayerRespawning()
    {
        //CHANGE/UPDATE : THiS
        SetState(PlayerState.Idle);
    }

    public void SlashColliderEnable()
    {
        _attackCollider2D.enabled = true;
    }
    public void DisableAttackCollider()
    {
        _attackCollider2D.enabled = false;
    }
}
