using UnityEngine;

public class EnableAxeCollider : MonoBehaviour
{
    private BoxCollider2D axeCollider;

    private void Awake()
    {
        axeCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Breakable"))
        {
            Breakable breakable = collision.GetComponent<Breakable>();
            if(breakable != null)
            {
                breakable.Hurt(5); // Example damage value
            }
        }
    }

    public void EnableCollider()
    {
        axeCollider.enabled = true;
    }

    public void DisableCollider()
    {
        axeCollider.enabled = false;
    }
}
