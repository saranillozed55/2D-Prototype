using UnityEngine;

public class MinotaurController : GroundEnemy
{
    
    private void FixedUpdate()
    {
        if (isKnockbacked || isDead) return;
        _stateMachine.Update();
    } 
}
