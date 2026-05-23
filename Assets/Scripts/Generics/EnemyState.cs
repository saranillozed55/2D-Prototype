using UnityEngine;

public abstract class EnemyState<T> : IState where T: MonoBehaviour
{

    protected T enemy;
    protected StateMachine stateMachine;

    public EnemyState(T enemy, StateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

}
