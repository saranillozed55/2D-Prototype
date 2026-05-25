using System.Collections;
using UnityEngine;

public class DeathState : EnemyState<TestEnemy>
{
    public DeathState(TestEnemy enemy, StateMachine stateMachine) : base(enemy,stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered Death State");
        enemy._rb.linearVelocity = Vector2.zero;
        enemy._rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        enemy.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        enemy.StartCoroutine(BasicEnemyDeathRoutine());
    }

    public override void Update()
    {

    }
    public override void Exit()
    {
        Debug.Log("Leaving Death State");
    }

    private IEnumerator BasicEnemyDeathRoutine()
    {
        yield return new WaitForSeconds(enemy.DeathTimer);

        Object.Destroy(enemy.gameObject);
    }
}
