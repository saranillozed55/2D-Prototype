using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BatIdleState : IState
{
    private BatController _batController;
    private StateMachine _stateMachine;

    public BatIdleState(BatController batController, StateMachine stateMachine)
    {
        _batController = batController;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Bat is currently in idle state");
    }

    public void Update()
    {
        DetectIdleRange();
        //TODO: Implement animations and sound
    }

    private void DetectIdleRange()
    {

        Vector3 direction = _batController.PlayerTransform.position - _batController.transform.position;
        float distance = direction.magnitude;

        //must include player in obstacle layers because that we are checking if we are hitting the player.
        RaycastHit2D hit = Physics2D.Raycast(_batController.transform.position, direction.normalized, distance, _batController.ObstacleLayers);

        if (distance < _batController.IdleDetectRange && hit.collider != null && hit.transform == _batController.PlayerTransform)
        {
            //Player is in range
            _stateMachine.TransitionTo(new BatChaseState(_batController, _stateMachine));
        }
    }

    public void Exit()
    {
        Debug.Log("Bat is leaving idle state");
    }
}
