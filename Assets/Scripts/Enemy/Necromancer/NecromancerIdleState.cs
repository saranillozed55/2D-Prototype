using UnityEngine;

public class NecromancerIdleState : EnemyState<NecromancerBoss>
{


    public NecromancerIdleState(NecromancerBoss enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Necromancer has switched to its idle state!");
        enemy._animator.SetBool(NecromancerBoss.IsIdleHash, true);
    }

    public override void Exit()
    {
        Debug.Log("Necromancer has left its idle state!");
        enemy._animator.SetBool(NecromancerBoss.IsIdleHash, false);
    }

    public override void Update()
    {
        
    }
}
