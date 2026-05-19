using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Listen to Event Channels")]
    [SerializeField] private VoidEventChannelSO _onPlayerDeath;
    [SerializeField] private VoidEventChannelSO _onPlayerRespawn;
    [SerializeField] private FloatEventChannelSO _onPlayerInvincible;

    //references
    private PlayerController playerController;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private int _currentState;
    private float _lockedTill;

    private static readonly int IsRunningHash = Animator.StringToHash("MartialHero_Run");
    private static readonly int IsIdleHash = Animator.StringToHash("MartialHero_Idle");
    private static readonly int IsFallingHash = Animator.StringToHash("MartialHero_Fall");
    private static readonly int IsJumpingHash = Animator.StringToHash("MartialHero_Jump");
    private static readonly int IsDeadHash = Animator.StringToHash("MartialHero_Death");
    private static readonly int IsHitHash = Animator.StringToHash("MartialHero_Hit");
    private static readonly int IsAttackingHash1 = Animator.StringToHash("MartialHero_NewAttack");
    private static readonly int IsAttackingHash2 = Animator.StringToHash("MartialHero_Attack2");


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _onPlayerDeath.OnEventRaised += HandlePlayerDeath;
        _onPlayerRespawn.OnEventRaised += HandlePlayerRespawn;
        _onPlayerInvincible.OnEventRaised += HandleInvincibilityFlash;
        playerController.AttackEvent += HandleAttack;
    }

    private void OnDisable()
    {
        _onPlayerDeath.OnEventRaised -= HandlePlayerDeath;
        _onPlayerRespawn.OnEventRaised -= HandlePlayerRespawn;
        _onPlayerInvincible.OnEventRaised -= HandleInvincibilityFlash;
        playerController.AttackEvent -= HandleAttack;

    }
    private void Update()
    {
        var state = GetState();
        if (state == _currentState) return;
        animator.CrossFade(state, 0, 0);
        _currentState = state;
    }


    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        //if(playerController.state == PlayerState.Attacking) - This has to be determined by which attack we are performing
        if(!playerController.isGrounded && playerController.rb.linearVelocity.y < 0) return IsFallingHash;
        if(!playerController.isGrounded && playerController.rb.linearVelocity.y > 0) return IsJumpingHash;
        if(playerController.state == PlayerState.Running)
        {
            return IsRunningHash;
        }

        return IsIdleHash;
    }

    private void HandleAttack()
    {
        _currentState = LockState(IsAttackingHash1, 0.45f);
        animator.CrossFade(IsAttackingHash1, 0, 0);
    }

    private int LockState(int state, float time)
    {
        _lockedTill = Time.time + time;
        return state;
    }

    private void HandlePlayerDeath()
    {
        if (_currentState == IsDeadHash) return;
        _currentState = LockState(IsDeadHash, float.MaxValue); // locks forever
        animator.CrossFade(IsDeadHash, 0, 0);
    }
    private void HandlePlayerRespawn()
    {
        _lockedTill = 0f; // unlock the state
        _currentState = IsIdleHash;
        animator.CrossFade(IsIdleHash, 0, 0);
    }
    
    private void HandleInvincibilityFlash(float duration)
    {
        _currentState = LockState(IsHitHash, duration);
        animator.CrossFade(IsHitHash, 0, 0);
        StartCoroutine(FlashRoutine(duration));
    }

    private IEnumerator FlashRoutine(float duration)
    {
        Color originalColor = spriteRenderer.color;
        Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        for (int i = 0; i < duration; i++)
        {
            // add color change here for flashing effect
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(playerController.FlashDuration);
            //reset color here
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(playerController.FlashDuration);
        }
    }
}
