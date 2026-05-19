using System;
using UnityEngine;
using UnityEngine.Pool;

public class CollectableBase : MonoBehaviour
{
    [Header("Collectable Settings")]
    [SerializeField] protected float despawnTimer;

    /*
     * This should change depending on the type of currency. Blue, Gold, Silver give different number of coins.
     */
    [SerializeField] protected int numCoinsInStack; 

    [Header("Audio")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip sound;

    [Header("Dropped Settings")]
    [SerializeField] protected float rotationSpeed; 

    public event Action OnCollected; //Use for inventory/UI

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            Debug.Log("Collected");
            OnCollected?.Invoke();

            //FIX: Update this so that it can add however much that stack has
        }
    }
}
