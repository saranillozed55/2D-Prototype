using UnityEngine;

public class ChaseState : IState
{
    private TestEnemy _enemy;
    private StateMachine _stateMachine;

    public ChaseState(TestEnemy enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering Chase State");
    }

    public void Update()
    {
        ChasePLayer();
        CheckLoseAggro();
    }

    public void Exit()
    {
        Debug.Log("Exiting Chase State");
    }

    private void ChasePLayer()
    {
        float directionX = Mathf.Sign(_enemy.PlayerLocation.position.x - _enemy._enemyRb.position.x);    
        _enemy._enemyRb.linearVelocity = new Vector2(directionX * _enemy.MoveSpeed, _enemy._enemyRb.linearVelocity.y);
    }

    private void CheckLoseAggro()
    {
        float distanceToPlayer = Vector2.Distance(_enemy.transform.position, _enemy.PlayerLocation.position);

        if(distanceToPlayer > _enemy.LoseAggroRange)
        {
            _stateMachine.TransitionTo(new PatrolState(_enemy, _stateMachine));
        }
    }
}
