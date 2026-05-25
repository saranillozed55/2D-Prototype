using System.Collections;
using UnityEngine;

public class PatrolState : EnemyState<TestEnemy>
{
    private int _currentWayPointIndex = 0;
    private bool _isWaiting = false;
    private bool _isSpotting;

    private float edgePauseDuration = 1.5f;
    private float _enemySpottedPause = 1f;

    public PatrolState(TestEnemy enemy, StateMachine stateMachine) : base(enemy,stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Patrol State");
        _isSpotting = false;
    }

    public override void Update()
    {
        EnemyPatrol();
        CheckIfTargetIsInCone();
    }
    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }

    private void EnemyPatrol()
    {
        if(enemy.EnemyPath.wayPoints.Count == 0|| _isWaiting)
        {
            return;
        }

        Vector2 targetPos = enemy.EnemyPath.wayPoints[_currentWayPointIndex].position;

        //horizontal direction only
        float directionX = Mathf.Sign(targetPos.x - enemy._rb.position.x);

        enemy._rb.linearVelocity = new Vector2(directionX * enemy.MoveSpeed, enemy._rb.linearVelocity.y);

        if(Mathf.Abs(enemy._rb.position.x - targetPos.x) < 0.2f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % enemy.EnemyPath.wayPoints.Count;
            enemy.StartCoroutine(PatrolEdgePauseRoutine());
        }
    }

    private void CheckIfTargetIsInCone()
    {
        if (_isSpotting) return; // guard from couroutine spamming
        if (!enemy.IsTargetInCone()) return;

        _isSpotting = true;
        enemy.StartCoroutine(PlayerSpottedRoutine());
    }

    private IEnumerator PatrolEdgePauseRoutine()
    {
        _isWaiting = true;
        enemy._rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(edgePauseDuration);
        _isWaiting = false;
    }

    private IEnumerator PlayerSpottedRoutine()
    {
        enemy.NotifySpottedPlayer();
        enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
        yield return new WaitForSeconds(_enemySpottedPause);
        stateMachine.TransitionTo(new ChaseState(enemy, stateMachine));
    }
}
