using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


/*
 * IMPLEMENT: Context Steering
 */
public class BatChaseState : IState
{
    private BatController _batController;
    private StateMachine _stateMachine;

    public BatChaseState(BatController batController, StateMachine stateMachine)
    {
        _batController = batController;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Bat has switched to its chase state!");
    }

    public void Update()
    {
        IsPlayerStillInRange();
    }

    private void IsPlayerStillInRange()
    {
        Vector3 direction = _batController.PlayerTransform.position - _batController.transform.position;
        float distance = direction.magnitude;

        // when player gets far away
        if(distance > _batController.ChaseRange)
        {
            Debug.Log("Player is now out of range so switching states. Must change state so this might run forever");
            _stateMachine.TransitionTo(new BatIdleState(_batController, _stateMachine));
        }
    }

    public void Exit()
    {
        Debug.Log("Bat has left chase state!");
    }

}
