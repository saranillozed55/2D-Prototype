using System.Collections;
using UnityEngine;

public class MinotaurChaseState : EnemyState<MinotaurController>
{

    //REDO THE ENTIRE SCRIPT
    private float _searchTimer;
    private bool _isAttacking;
    private bool _lostPlayer;
    private int _currentState;
    

    private static readonly int IsWalkingHash = Animator.StringToHash("Minotaur_Walk_Anim");
    private static readonly int IsIdleHash = Animator.StringToHash("Minotaur_Idle_Anim");
    public MinotaurChaseState(MinotaurController enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        enemy.IsChasing = true;
        _lostPlayer = false;
        _isAttacking = false;
        _searchTimer = enemy.SearchDuration;
        _currentState = IsWalkingHash;
        enemy._animator.CrossFade(IsWalkingHash, 0, 0);
        Debug.Log("Minotaur in Chase State");
    }

    public override void Update()
    {
        CheckIfStillAggro();

        if (!_lostPlayer)
        {
            ChasePlayer();
            CheckAttack();
        }
        else
        {
            enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
        }

        int desiredState = GetState();
        if(desiredState != _currentState)
        {
            _currentState = desiredState;
            enemy._animator.CrossFade(desiredState, 0, 0);
        }
    }

    public override void Exit()
    {
        enemy.IsChasing = false;
        Debug.Log("Minotaur left Chase State");
    }
    
    private int GetState()
    {
        if (_lostPlayer) return IsIdleHash;
        return IsWalkingHash;
    }

    private void ChasePlayer() // update/fix this code
    {
        if (enemy.EdgeDetected || enemy.WallDetected)
        {
            enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
            if (!enemy.HasLastSeenPosition)
            {
                enemy.UpdateLastKnowPosition(enemy.PlayerLocation.position);
            }
            Debug.Log($"Ledge detect: {enemy.EdgeDetected}, Wall detect: {enemy.WallDetected}");
            return;
        }
        if (enemy.HasLastSeenPosition)
        {
            // move toward last seen position
            float xDir = Mathf.Sign(enemy.LastSeenPosition.x - enemy._rb.position.x);
            enemy._rb.linearVelocity = new Vector2(xDir * enemy.MoveSpeed, enemy._rb.linearVelocity.y);
        }
        else
        {
            //chase directly since player is still in view
            float directionX = Mathf.Sign(enemy.PlayerLocation.position.x - enemy._rb.position.x);
            enemy._rb.linearVelocity = new Vector2(directionX * enemy.MoveSpeed, enemy._rb.linearVelocity.y);
        }
    }

    private void CheckIfStillAggro()
    {
        float dis = DistToPlayer();

        if (dis > enemy.LoseAggroRange && !_lostPlayer)
        {
            enemy.StartCoroutine(LostPlayer());
            return;
        }

        Vector2 direction = (enemy.PlayerLocation.position - enemy.transform.position).normalized;

        int layerMask = (1 << enemy.PlayerLayer) | enemy.ObstacleLayer;
        Debug.DrawRay(enemy.transform.position, direction * dis, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, dis, layerMask);
        Debug.Log($"LOS hit: {hit.collider?.name}, EdgeDetected: {enemy.EdgeDetected}, WallDetected: {enemy.WallDetected}");

        if (hit.collider != null)
        {
            // Player is directly visible — clear any saved position and reset the search timer
            if (enemy.HasLastSeenPosition)
                enemy.ClearLastKnowPosition();

            _searchTimer = enemy.SearchDuration;
        }
        else
        {
            // Lost line of sight — record where they were last seen and start counting down
            if (!enemy.HasLastSeenPosition)
                enemy.UpdateLastKnowPosition(enemy.PlayerLocation.position);

            LostAggroTimer();
        }
    }

    private void CheckAttack()
    {
        float dis = DistToPlayer();
        if (dis <= enemy.AttackRange)
        {
            Debug.Log("Player is in attack range");
        }
    }

    private void LostAggroTimer()
    {
        _searchTimer -= Time.deltaTime;
        if(_searchTimer <= 0f)
        {
            enemy.ClearLastKnowPosition();
            stateMachine.TransitionTo(new MinotaurPatrolState(enemy, stateMachine));
        }
    }

    private float DistToPlayer()
    {
        return Vector2.Distance(enemy.transform.position, enemy.PlayerLocation.position);
    }

    private IEnumerator LostPlayer()
    {
        _lostPlayer = true;
        yield return new WaitForSeconds(1f);
        stateMachine.TransitionTo(new MinotaurPatrolState(enemy, stateMachine));
    }
}
