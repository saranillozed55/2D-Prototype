using UnityEngine;

public class CollectTrigger : MonoBehaviour
{
    private Coin coin;
    
    private void Awake()
    {
        coin = GetComponentInParent<Coin>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            coin.Collect(collider);
        }
    }
}
