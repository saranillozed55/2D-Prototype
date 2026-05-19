using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Health and Death/Breakable")]
    [SerializeField] protected int health;
    [SerializeField] protected bool isDead;

    public event Action OnDeath;
    protected void CheckIsDead()
    {
        if(health <= 0 && !isDead)
        {
            Debug.Log("Breakable Runs Check Death");
            Dead();
        }
    }


    protected virtual void Dead()
    {
        isDead = true;
        OnDeath?.Invoke();
    }

    public virtual void Hurt(int damage)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0) Dead();
    }
   
}
