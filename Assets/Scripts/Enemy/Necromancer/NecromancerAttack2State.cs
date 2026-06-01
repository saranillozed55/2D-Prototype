using UnityEngine;

public class NecromancerAttack2State : EnemyState<NecromancerBoss>
{
    public NecromancerAttack2State(NecromancerBoss enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Necromancer has switched to its attack 2 state!");
        enemy.StopBossMovement();
        enemy._animator.SetTrigger(NecromancerBoss.IsAttack2Hash);
        enemy.PreviousAttackState = this;
    }
    public override void Update()
    {
        //This is switching back to walk state after the attack animation is done in NecromancerBoss
    }
    public override void Exit()
    {
        Debug.Log("Necromancer has left its attack 2 state!");
    }
}
