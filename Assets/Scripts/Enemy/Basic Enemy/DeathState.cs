using System.Collections;
using UnityEngine;

public class DeathState : IState
{
    private TestEnemy _enemy;
    private StateMachine _stateMachine;

    public DeathState(TestEnemy enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entered Death State");
        _enemy._enemyRb.linearVelocity = Vector2.zero;
        _enemy._enemyRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        _enemy.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        _enemy.StartCoroutine(BasicEnemyDeathRoutine());
    }

    public void Update()
    {

    }
    public void Exit()
    {
        Debug.Log("Leaving Death State");
    }

    private IEnumerator BasicEnemyDeathRoutine()
    {
        yield return new WaitForSeconds(_enemy.DeathTimer);

        Object.Destroy(_enemy.gameObject);
    }
}
