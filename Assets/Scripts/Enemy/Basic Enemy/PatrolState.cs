using System.Collections;
using UnityEngine;

public class PatrolState : IState
{
    private TestEnemy _enemy;
    private StateMachine _stateMachine;
    private int _currentWayPointIndex = 0;
    private bool _isWaiting = false;
    private bool _isSpotting;

    private float edgePauseDuration = 1.5f;
    private float _enemySpottedPause = 1f;

    public PatrolState(TestEnemy enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering Patrol State");
        _isSpotting = false;
    }

    public void Update()
    {
        EnemyPatrol();
        CheckIfTargetIsInCone();
    }
    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }

    private void EnemyPatrol()
    {
        if(_enemy.EnemyPath.wayPoints.Count == 0|| _isWaiting)
        {
            return;
        }

        Vector2 targetPos = _enemy.EnemyPath.wayPoints[_currentWayPointIndex].position;

        //horizontal direction only
        float directionX = Mathf.Sign(targetPos.x - _enemy._enemyRb.position.x);

        _enemy._enemyRb.linearVelocity = new Vector2(directionX * _enemy.MoveSpeed, _enemy._enemyRb.linearVelocity.y);

        if(Mathf.Abs(_enemy._enemyRb.position.x - targetPos.x) < 0.2f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _enemy.EnemyPath.wayPoints.Count;
            _enemy.StartCoroutine(PatrolEdgePauseRoutine());
        }
    }

    private void CheckIfTargetIsInCone()
    {
        if (_isSpotting) return; // guard from couroutine spamming
        if (!_enemy.IsTargetInCone()) return;

        _isSpotting = true;
        _enemy.StartCoroutine(PlayerSpottedRoutine());
    }

    private IEnumerator PatrolEdgePauseRoutine()
    {
        _isWaiting = true;
        _enemy._enemyRb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(edgePauseDuration);
        _isWaiting = false;
    }

    private IEnumerator PlayerSpottedRoutine()
    {
        _enemy.NotifySpottedPlayer();
        _enemy._enemyRb.linearVelocity = new Vector2(0, _enemy._enemyRb.linearVelocity.y);
        yield return new WaitForSeconds(_enemySpottedPause);
        _stateMachine.TransitionTo(new ChaseState(_enemy, _stateMachine));
    }
}
