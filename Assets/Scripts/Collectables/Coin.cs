using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : CollectableBase
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private ObjectPool<GameObject> objectPool;
    //public property to give the coin a reference to its ObjectPool
    public ObjectPool<GameObject> ObjectPool { set => objectPool = value;}

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 2f;
    [SerializeField] private float timeOutDelay = 5f;
    [SerializeField] private float flashSpeed = 0.1f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //CAN: Remove Rotation if visuals need
    private void Update()
    {
        //rotate coin
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(collider);
            CurrencyManager.Instance.AddCoins(numCoinsInStack);
            AudioSource.PlayClipAtPoint(sound, transform.position); // play sound at world position
            StopAllCoroutines(); // stop despawn/flash timer
            spriteRenderer.enabled = true; // reset in case it was flashing
            objectPool.Release(gameObject);
        }
    }

    private void Deactivate()
    {
        StartCoroutine(DeactivateRoutine());
    }

    private IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(timeOutDelay - flashDuration);

        float elapsedTime = 0f;
        while(elapsedTime < flashDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashSpeed);
            elapsedTime += flashSpeed;
        }

        objectPool.Release(gameObject);
    }

    public void OnSpawn(float flingForce)
    {
        float randomAngle = Random.Range(20f, 160f);
        Vector2 randomdir = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

        rb.AddForce(randomdir * flingForce, ForceMode2D.Impulse);
        Deactivate();
    }

    public void OnRelease()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
