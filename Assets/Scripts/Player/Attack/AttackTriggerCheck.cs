using UnityEngine;

public class AttackTriggerCheck : MonoBehaviour
{
    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger Hit: {collision.gameObject.name} | tag: {collision.tag}");
        if(collision.CompareTag("Breakable"))
        {
            var breakable = collision.GetComponent<Breakable>();
            if (breakable != null)
            {
                breakable.Hurt(5, transform.position); // Change this value to not be a magic number
            }
            else
            {
                Debug.LogWarning("No Breakable component found on " + collision.gameObject.name);
            }
        }
    }
}
