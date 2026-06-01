using UnityEngine;

public class MinotaurController : GroundEnemy
{
    private MinotaurPatrolState m_patrolState;
    
    protected override void Start()
    {
        base.Start();

        m_patrolState = new MinotaurPatrolState(this, _stateMachine);
        _stateMachine.Initialize(m_patrolState);
    }
    private void FixedUpdate()
    {
        if (isKnockbacked || isDead) return;
        _stateMachine.Update();
    } 
}
