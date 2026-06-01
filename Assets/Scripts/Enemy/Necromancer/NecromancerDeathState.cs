using UnityEngine;

public class NecromancerDeathState : EnemyState<NecromancerBoss>
{
    private float _timer;
    private float _deathDuration;
    public NecromancerDeathState(NecromancerBoss enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
        _deathDuration = 5f; // Set the duration for the death animation
    }
    public override void Enter()
    {
        Debug.Log("Necromancer has died and is in deaht state");
        enemy.StopBossMovement();
        enemy._animator.SetTrigger(NecromancerBoss.IsDeadHash);
        _timer = 0f;
    }
    public override void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _deathDuration)
        {
            Object.Destroy(enemy.gameObject);
        }
    }

    public override void Exit()
    {

    }
}
