using System.IO;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BatChaseState : EnemyState<BatController>
{

    private static readonly int IsFlyingHash = Animator.StringToHash("Bat_Fly_Anim");
    private float _startY; //anchor line for the floating effect

    public BatChaseState(BatController enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Bat has switched to its chase state!");
        _startY = enemy.transform.position.y;
        enemy._animator.CrossFade(IsFlyingHash, 0, 0);
    }

    public override void Update()
    {
        BatFloating();
        IsPlayerStillInRange();
        //TODO: Context Steering
    }

    private void BatFloating()
    {
        Vector2 direction = (enemy.PlayerTransform.position - enemy.transform.position).normalized;

        //Head toward the player at our movespeed
        Vector2 chaseVelocity = direction * enemy.MoveSpeed;

        //Cos here because it represents ROC(velocity) of a wave
        float waveSpeed = Mathf.Cos(Time.time * enemy.FloatingFrequency) * enemy.FloatingAmplitude;
        Vector2 floatVelocity = new Vector2(0, waveSpeed);

        enemy._batRb.linearVelocity = chaseVelocity + floatVelocity;
    }

    private void IsPlayerStillInRange()
    {
        Vector3 direction = enemy.PlayerTransform.position - enemy.transform.position;
        float distance = direction.magnitude;

        // when player gets far away
        if(distance > enemy.ChaseRange)
        {
            Debug.Log("Player is now out of range so switching states. Must change state so this might run forever");
            stateMachine.TransitionTo(new BatIdleState(enemy, stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("Bat has left chase state!");
    }

}
