using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Health and Death/Breakable")]
    [SerializeField] protected int health;
    [SerializeField] protected bool isDead;

    public event Action OnDeath;

    public int Health => health;
    public int MaxHealth { get; private set;} // use for reviving, health caps, and health bars

    protected virtual void Awake()
    {
        MaxHealth = health;
    }

    protected virtual void Dead()
    {
        isDead = true;
        OnDeath?.Invoke();
    }

    public virtual void Hurt(int damage, Vector2 hitDirection)
    {
        if (isDead) return;
        health -= damage;
        OnHurt();
        if (health <= 0) Dead();
    }

    protected virtual void OnHurt()
    {
        //implement specific code here
    }
   
}
