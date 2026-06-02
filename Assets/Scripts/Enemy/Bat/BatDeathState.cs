using UnityEngine;

public class BatDeathState : EnemyState<BatController>
{
    public BatDeathState(BatController enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        enemy._animator.SetTrigger(BatController.IsDeadHash);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }

}
