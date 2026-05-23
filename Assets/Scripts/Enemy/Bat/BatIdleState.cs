using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BatIdleState : EnemyState<BatController>
{
    //sending enemy and stateMachine to parent class

    private static readonly int IsIdleHash = Animator.StringToHash("Bat_Idle_Anim");
    public BatIdleState(BatController enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Bat is currently in idle state");
        enemy._animator.CrossFade(IsIdleHash, 0, 0);
    }

    public override void Update()
    {
        DetectIdleRange();
        //TODO: Implement animations and sound
    }

    private void DetectIdleRange()
    {

        Vector3 direction = enemy.PlayerTransform.position - enemy.transform.position;
        float distance = direction.magnitude;

        //must include player in obstacle layers because that we are checking if we are hitting the player.
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction.normalized, distance, enemy.ObstacleLayers);

        if (distance < enemy.IdleDetectRange && hit.collider != null && hit.transform == enemy.PlayerTransform)
        {
            //Player is in range
            stateMachine.TransitionTo(new BatChaseState(enemy, stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("Bat is leaving idle state");
    }
}
