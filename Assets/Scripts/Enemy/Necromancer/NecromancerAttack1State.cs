using System.Threading;
using UnityEngine;

public class NecromancerAttack1State : EnemyState<NecromancerBoss>
{
    private float _timer;
    private float _maxDuration = 2f; // Duration of the attack animation, adjust as needed

    public NecromancerAttack1State(NecromancerBoss enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override void Enter()
    {
        enemy.StopBossMovement();
        enemy._animator.SetTrigger(NecromancerBoss.IsAttack1Hash);
        enemy.PreviousAttackState = this;
        _timer = 0f;
    }
    public override void Exit()
    {
    }
    public override void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _maxDuration)
        {
            stateMachine.TransitionTo(new NecromancerWalkState(enemy, stateMachine));
        }
    }
}