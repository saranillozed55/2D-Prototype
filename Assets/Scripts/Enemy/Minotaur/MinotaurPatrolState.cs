using UnityEngine;

public class MinotaurPatrolState : GroundEnemyPatrolState<MinotaurController>
{

    private static readonly int IsWalking = Animator.StringToHash("Minotaur_Walk_Anim");
    private static readonly int IsIdle = Animator.StringToHash("Minotaur_Idle_Anim");

    public MinotaurPatrolState(MinotaurController enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

    protected override void OnEnterAnimation()
    {
        enemy._animator.CrossFade(IsWalking, 0, 0);
    }
    protected override void OnWalkAnimations()
    {
        enemy._animator.CrossFade(IsWalking, 0, 0);
    }
    protected override void OnIdleAnimation()
    {
        enemy._animator.CrossFade(IsIdle, 0, 0);
    }

    protected override void OnSpottedAnimations()
    {
        enemy._animator.CrossFade(IsIdle, 0, 0);
    }

    //protected override EnemyState<MinotaurController> GetChaseState() => new MinoutaurChaseState(enemy, stateMachine);
}
