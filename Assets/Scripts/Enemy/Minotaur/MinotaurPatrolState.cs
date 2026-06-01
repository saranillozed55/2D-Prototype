using UnityEngine;
using System.Collections;

public class MinotaurPatrolState : EnemyState<MinotaurController>
{
    private bool _isWaiting = false;
    private bool _isPlayerSpotted = false;
    private int _currentState;

    private static readonly int IsTauntHash = Animator.StringToHash("Minotaur_Taunt_Anim");
    private static readonly int IsWalkingHash = Animator.StringToHash("Minotaur_Walk_Anim");
    private static readonly int IsIdleHash = Animator.StringToHash("Minotaur_Idle_Anim");

    public MinotaurPatrolState(MinotaurController enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        _isWaiting = false;
        _isPlayerSpotted = false;
        _currentState = IsWalkingHash;
        enemy._animator.CrossFade(IsWalkingHash, 0, 0);
    }

    public override void Update()
    {
        if (_isPlayerSpotted)
        {
            enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
        }

        Patrol();
        CheckCone();

        // Only crossfade when desired animation changes
        // Taunt is excluded here since it's driven directly by the coroutine
        int desiredState = GetState();
        if (desiredState != _currentState)
        {
            _currentState = desiredState;
            enemy._animator.CrossFade(desiredState, 0, 0);
        }
    }

    public override void Exit()
    {
        Debug.Log("Minotaur Leaving Patrol State");
    }

    private int GetState()
    {
        if (_isPlayerSpotted) return IsTauntHash;
        if (_isWaiting) return IsIdleHash;
        return IsWalkingHash;
    }

    private void Patrol()
    {
        if (enemy.EnemyPath.wayPoints.Count == 0 || _isWaiting || _isPlayerSpotted) return;

        Vector2 targetPos = enemy.EnemyPath.wayPoints[enemy._currentWayPointIndex].position;

        float directionX = Mathf.Sign(targetPos.x - enemy._rb.position.x);
        enemy._rb.linearVelocity = new Vector2(directionX * enemy.MoveSpeed, enemy._rb.linearVelocity.y);

        if (Mathf.Abs(targetPos.x - enemy._rb.position.x) < 0.2f)
        {
            enemy._currentWayPointIndex = (enemy._currentWayPointIndex + 1) % enemy.EnemyPath.wayPoints.Count;
            enemy.StartCoroutine(PauseRoutine());
        }
    }

    private void CheckCone()
    {
        if (_isPlayerSpotted || !enemy.IsTargetInCone()) return;
        enemy.StartCoroutine(PlayerSpottedRoutine());
    }

    private IEnumerator PauseRoutine()
    {
        _isWaiting = true;
        enemy._rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(enemy.EdgePauseDuration);
        _isWaiting = false;
    }

    private IEnumerator PlayerSpottedRoutine()
    {
        enemy.NotifySpottedPlayer();
        _isPlayerSpotted = true;

        // Taunt is triggered here directly — _currentState updated to stay in sync
        _currentState = IsTauntHash;
        enemy._animator.CrossFade(IsTauntHash, 0, 0);

        yield return new WaitForSeconds(enemy.EnemySpottedPause);
        stateMachine.TransitionTo(new MinotaurChaseState(enemy, stateMachine));
    }
}