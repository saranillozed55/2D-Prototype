using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class KnockbackState : IState
{
    private TestEnemy _enemy;
    private StateMachine _stateMachine;

    public KnockbackState(TestEnemy enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Enemy was attacked. Knockback state entered.");
        _enemy.StartCoroutine(KnockbackRoutine());
    }

    public void Update()
    {

    }
    public void Exit()
    {
        Debug.Log("Knockback state exited.");
    }

    private IEnumerator KnockbackRoutine()
    {
        _enemy._enemyRb.linearVelocity = Vector2.zero; // stop movement
        _enemy._enemyRb.linearVelocity = -_enemy.transform.right * _enemy.KnockbackForce; // apply knockback force in the opposite direction of the enemy's facing direction
        _enemy.isKnockbacked = true;
        yield return new WaitForSeconds(_enemy.KnockBackDuration); // wait for knockback duration
        _stateMachine.TransitionTo(new ChaseState(_enemy, _stateMachine)); // transition back to patrol state after knockback
    }
}
