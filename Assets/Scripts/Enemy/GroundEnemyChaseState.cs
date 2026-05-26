using UnityEngine;

public class GroundEnemyChaseState<T> : EnemyState<T> where T : GroundEnemy
{

    public GroundEnemyChaseState(T enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }



}

