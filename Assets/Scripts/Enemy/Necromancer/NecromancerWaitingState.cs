using UnityEngine;

public class NecromancerWaitingState : EnemyState<NecromancerBoss>
{
    public NecromancerWaitingState(NecromancerBoss enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Necromancer started in Waiting State!");
        enemy._animator.SetBool(NecromancerBoss.IsIdleHash, true);
    }

    public override void Exit()
    {
        Debug.Log("Necromancer has left its waiting state!");
        enemy._animator.SetBool(NecromancerBoss.IsIdleHash, false);
    }

    public override void Update()
    {
    }
}
