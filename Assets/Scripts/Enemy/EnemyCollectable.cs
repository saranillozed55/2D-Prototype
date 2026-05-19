
using UnityEngine;
public class EnemyCollectable : MonoBehaviour
{
    [Header("Enemy Drops Settings")]
    [SerializeField] private int maxDrops; //Should scale on enemy difficulty/size

    [SerializeField] private GameObject coinPrefab;

    private Breakable breakable;

    private void Awake()
    {
        breakable = GetComponent<Breakable>();
    }

    private void OnEnable()
    {
        breakable.OnDeath += SpawnCollectables;
    }

    private void OnDisable()
    {
        breakable.OnDeath -= SpawnCollectables;
    }

    private void SpawnCollectables()
    {
        int amount = Random.Range(1, maxDrops);

        Debug.Log("Spawning Collectables");
        for(int i = 0; i < amount; i++)
        {
            Debug.Log(i);
            FlingCollectibles();
        }
    }

    private void FlingCollectibles()
    {
        CoinPool.Instance.Get(transform.position);
    }
    
}
