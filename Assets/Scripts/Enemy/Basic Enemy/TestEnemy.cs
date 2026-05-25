using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TestEnemy : GroundEnemy
{
    public BoxCollider2D PhysicsCollider { get; private set; }
    public BoxCollider2D HurtBox { get; private set; }

    private PatrolState _patrolState;

    protected override void Start()
    {
        base.Start();
        PhysicsCollider = GetComponent<BoxCollider2D>();
        HurtBox = GetComponentInChildren<BoxCollider2D>();

        _patrolState = new PatrolState(this, _stateMachine);
        _stateMachine.Initialize(_patrolState);
    }

    private void FixedUpdate()
    {
        if (isKnockbacked || isDead) return; // prevent state updates during knockback
        _stateMachine.Update();
    }

    //Refactor so that any state knows about the player always faces them, and only patrol uses velocity-based facing
    //Refactor: Add this to Enemy instead because all enemies will have to turn to the enemy.
    protected override void HandleTurn()
    {
        //if alerting or chasing, face the player directly
        if (_stateMachine.CurrentState is ChaseState)
        {
            float directionToPlayer = _playerTransform.position.x - transform.position.x;

            _isFacingRight = directionToPlayer > 0;

            float angle = directionToPlayer > 0 ? 0 : 180f;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        else
        {
            base.HandleTurn();
        }

    }


}
