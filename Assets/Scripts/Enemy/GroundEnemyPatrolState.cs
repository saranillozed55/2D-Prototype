//using System.Collections;
//using UnityEngine;

//public class GroundEnemyPatrolState<T> : EnemyState<T> where T : GroundEnemy
//{
//    private bool _isWaiting = false;
//    private bool _isSpotting = false;
//    public GroundEnemyPatrolState(T enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }


//    protected virtual int IsMovingHash() => 0;

//    public override void Enter()
//    {
//        _isWaiting = false;
//        _isSpotting = false;
//        enemy._animator.SetBool(IsMovingHash(), true);
//    }

//    public override void Update()
//    {
//        Patrol();
//        CheckCone();
//    }

//    public override void Exit()
//    {
//        Debug.Log("Ground Enemy has left patrol state");
//    }

//    private void Patrol()
//    {
//        if (enemy.EnemyPath.wayPoints.Count == 0 || _isWaiting || _isSpotting)
//        {
//            return;
//        }

//        Vector2 targetPos = enemy.EnemyPath.wayPoints[enemy._currentWayPointIndex].position;

//        //move in horizontal direction only
//        float directionX = Mathf.Sign(targetPos.x - enemy._rb.position.x);

//        enemy._rb.linearVelocity = new Vector2(directionX * enemy.MoveSpeed, enemy._rb.linearVelocity.y);

//        if (Mathf.Abs(targetPos.x - enemy._rb.position.x) < 0.2f)
//        {
//            enemy._currentWayPointIndex = (enemy._currentWayPointIndex + 1) % enemy.EnemyPath.wayPoints.Count;
//            enemy.StartCoroutine(PauseRoutine());
//        }
//    }
//    private void CheckCone()
//    {
//        if (_isSpotting || !enemy.IsTargetInCone()) return;
//        _isSpotting = true;
//        enemy.StartCoroutine(SpottedRoutine());
//    }
//    private IEnumerator PauseRoutine()
//    {
//        _isWaiting = true;
//        enemy._rb.linearVelocity = Vector2.zero; // can cause problems if enemy is on moving platform
//        enemy._animator.SetBool(IsMovingHash(), false);
//        yield return new WaitForSeconds(enemy.EdgePauseDuration);
//        enemy._animator.SetBool(IsMovingHash(), true);
//        _isWaiting = false;
//    }

//    private IEnumerator SpottedRoutine()
//    {
//        enemy.NotifySpottedPlayer();
//        enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
//        enemy._animator.SetBool(IsMovingHash(), false);
//        yield return new WaitForSeconds(enemy.EnemySpottedPause);

//        var chaseState = GetChaseState();
//        Debug.Log(chaseState.ToString());
//        if(chaseState != null)
//        {
//            stateMachine.TransitionTo(chaseState);
//        }
//    }
//    protected virtual EnemyState<T> GetChaseState() => null;

//}
