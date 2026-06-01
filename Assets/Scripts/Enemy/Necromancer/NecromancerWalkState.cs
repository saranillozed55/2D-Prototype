using UnityEngine;

public class NecromancerWalkState : EnemyState<NecromancerBoss>
{
    private bool _isPlayerInFront;

    public NecromancerWalkState(NecromancerBoss enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        enemy._animator.SetBool(NecromancerBoss.IsWalkHash, true);
    }

    public override void Exit()
    {
        enemy._animator.SetBool(NecromancerBoss.IsWalkHash, false);
    }

    public override void Update()
    {
        _isPlayerInFront = IfPlayerIsInfront();
        //Debug.Log(_isPlayerInFront);
        WalkTowardsPlayer();
        CheckIfClose();
        HandleTurning();
    }

    private void WalkTowardsPlayer()
    {
        float xDir = Mathf.Sign(enemy.PlayerTransform.position.x - enemy.transform.position.x);
        enemy._rb.linearVelocity = new Vector2(xDir * enemy.MoveSpeed, enemy._rb.linearVelocity.y);
    }
    private void CheckIfClose()
    {
        //Would like to maybe determine which attack state to switch based on distance but for now just switch to attack state when close enough
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.PlayerTransform.position);
        if (distanceToPlayer <= enemy.Attack1Range && _isPlayerInFront)
        {
            if (enemy.PreviousAttackState is NecromancerAttack1State)
            {
                stateMachine.TransitionTo(new NecromancerAttack2State(enemy, stateMachine));
                return;
            }
            else
            {
                stateMachine.TransitionTo(new NecromancerAttack1State(enemy, stateMachine));
            }
        }
    }

    private bool IfPlayerIsInfront()
    {
        Vector2 directionToPlayer = (enemy.PlayerTransform.position - enemy.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, enemy.transform.right, enemy.Attack1Range, enemy.PlayerLayerMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void HandleTurning()
    {
        if (Mathf.Abs(enemy._rb.linearVelocity.x) > 0.1f)
        {
            float angle;
            if (enemy._rb.linearVelocity.x > 0)
            {
                angle = 0f;
            }
            else
            {
                angle = 180f;
            }
            enemy.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

}
