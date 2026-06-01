//using System.Collections;
//using UnityEngine;

//public class GroundEnemyChaseState<T> : EnemyState<T> where T : GroundEnemy
//{
//    private bool _isQuickLost; // maybe don't have quick lost, rather just have the enemy stand at its current position then go back after a timer
//    private float _searchTimer;

//    public GroundEnemyChaseState(T enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

//    public virtual int IsMovingHash() => 0;
//    public virtual int IsAttack1Hash() => 0;

//    public override void Enter()
//    {
//        _isQuickLost = false;
//        enemy._animator.SetBool(IsMovingHash(), true);
//    }

//    public override void Update()
//    {
//        CheckLostAggro();
//        AttackCheck();
//        ChasePlayer();
//    }

//    public override void Exit()
//    {
//        Debug.Log($"Exiting Chase State: {enemy}");
//    }

//    private void ChasePlayer()
//    {
//        if (enemy.EdgeDetected || enemy.WallDetected)
//        {

//            enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
//            if (!enemy.HasLastSeenPosition)
//            {
//                enemy.UpdateLastKnowPosition(enemy.PlayerLocation.position);
//            }
//            Debug.Log($"Ledge detect: {enemy.EdgeDetected}, Wall detect: {enemy.WallDetected}");
//            return;
//        }
//        if (enemy.HasLastSeenPosition)
//        {
//            // move toward last seen position
//            float xDir = Mathf.Sign(enemy.LastSeenPosition.x - enemy._rb.position.x);
//            enemy._rb.linearVelocity = new Vector2(xDir * enemy.MoveSpeed, enemy._rb.linearVelocity.y);
//        }
//        else
//        {
//            //chase directly since player is still in view
//            float directionX = Mathf.Sign(enemy.PlayerLocation.position.x - enemy._rb.position.x);
//            enemy._rb.linearVelocity = new Vector2(directionX * enemy.MoveSpeed, enemy._rb.linearVelocity.y);
//        }
//    }

//    private void CheckLostAggro()
//    {
//        float dis = DistToPlayer();
//        Vector2 directionToPlayer = (enemy.PlayerLocation.position - enemy.transform.position).normalized;

//        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, directionToPlayer, dis, enemy.PlayerLayer);

//        if (dis > enemy.LoseAggroRange && !_isQuickLost)
//        {
//            enemy.StartCoroutine(QuickLost());
//        }

//        if(hit.collider == null && !enemy.HasLastSeenPosition)
//        {
//            enemy.UpdateLastKnowPosition(enemy.PlayerLocation.position);
//        }
//        if(enemy.HasLastSeenPosition)
//        {
//            LostAggroTimer();
//        }
//    }

//    private void AttackCheck()
//    {
//        float dis = DistToPlayer();
//        if(dis <= enemy.AttackRange)
//        {
//            var attackState = GetAttackState();
//            if (attackState != null)
//            {
//                stateMachine.TransitionTo(attackState);
//            }
//        }
//    }

//    private void LostAggroTimer()
//    {
//        _searchTimer -= Time.deltaTime;
//        if(_searchTimer <= 0f)
//        {
//            enemy.ClearLastKnowPosition();
//            var patrolState = GetPatrolState();
//            if(patrolState != null)
//            {
//                stateMachine.TransitionTo(patrolState);
//            }
//        }
//    }

//    private IEnumerator QuickLost()
//    {
//        _isQuickLost = true;
//        enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
//        yield return new WaitForSeconds(0.5f);
//        _isQuickLost = false;
//        //state machine transition
//        var patrolState = GetPatrolState();
//        if (patrolState != null)
//        {
//            stateMachine.TransitionTo(patrolState);
//        }
//    }

//    private float DistToPlayer()
//    {
//        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.PlayerLocation.position);
//        return distanceToPlayer;
//    }

//    protected virtual EnemyState<T> GetAttackState() => null;
//    protected virtual EnemyState<T> GetPatrolState() => null;
//}

