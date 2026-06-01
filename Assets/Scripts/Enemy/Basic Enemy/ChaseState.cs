using System.Collections;
using UnityEngine;

public class ChaseState : EnemyState<TestEnemy>
{

    private bool _isQuickLost;
    private float _searchTimer;

    public ChaseState(TestEnemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Chase State");
        _searchTimer = 3f;
    }

    public override void Update()
    {
        ChasePLayer();
        CheckLoseAggro();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Chase State");
    }

    private void ChasePLayer()
    {
        if (enemy.EdgeDetected || enemy.WallDetected) // maybe move this logic to somewhere else if we want the AI to move without patrol points
        {
            enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
            if(!enemy.HasLastSeenPosition)
            {
                enemy.UpdateLastKnowPosition(enemy.PlayerLocation.position);
            }

            Debug.Log($"Ledge detect: {enemy.EdgeDetected}, Wall detect: {enemy.WallDetected}");
            return; // stop moving but still have the last seen position
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
    private void CheckLoseAggro()
    {
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.PlayerLocation.position);
        Vector2 directionToPlayer = (enemy.PlayerLocation.position - enemy.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, directionToPlayer, enemy.LoseAggroRange, enemy.PlayerLayer);

        if(distanceToPlayer > enemy.LoseAggroRange && !_isQuickLost)
        {
            //lost by distance, so give up quickly
            enemy.StartCoroutine(QuickLost());
        }

        if (hit.collider == null && !enemy.HasLastSeenPosition)
        {
            enemy.UpdateLastKnowPosition(enemy.PlayerLocation.position);
            //Add wall detection here(I believe) and then add pause - maybe switch to new state if need
        }
        if(enemy.HasLastSeenPosition)
        {
            LostAgroTimer();
        }
    }
    private void LostAgroTimer()
    {
        _searchTimer -= Time.deltaTime;
        if(_searchTimer <= 0f)
        {
            enemy.ClearLastKnowPosition();
            stateMachine.TransitionTo(new PatrolState(enemy, stateMachine));
        }
    }
    private IEnumerator QuickLost()
    {
        _isQuickLost = true;
        enemy._rb.linearVelocity = new Vector2(0, enemy._rb.linearVelocity.y);
        yield return new WaitForSeconds(0.5f);
        _isQuickLost = false;
        stateMachine.TransitionTo(new PatrolState(enemy, stateMachine));
    }
}
