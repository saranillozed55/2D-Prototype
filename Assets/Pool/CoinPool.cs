using UnityEngine;
using UnityEngine.Pool;

public class CoinPool : GenericSingleton<CoinPool>
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 50;
    [SerializeField] private float flingForce = 5f;

    private ObjectPool<GameObject> pool;

    protected override void Awake()
    {
        base.Awake();
        pool = new ObjectPool<GameObject>(createFunc: CreateCoin,
            actionOnGet: OnGetCoin,
            actionOnRelease: OnReleaseCoin,
            actionOnDestroy: OnDestroyCoin,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
            );
    }

    private void OnDisable()
    {
        pool?.Clear();
    }

    /*
     * Called by pool automatically when it needs a new coin that doesn't exist yet. Gets the 'Coin' componenet and assigns the pool refernce
     * So the coin can release itself later
     * */
    private GameObject CreateCoin()
    {
        var coinObject = Instantiate(coinPrefab);
        var coin = coinObject.GetComponent<Coin>();

        coin.ObjectPool = pool;

        return coinObject;
    }

    /*
     * Called Every time a coin is retreived from the pool via pool.Get() in EnemyCollectable and such.
     */

    private void OnGetCoin(GameObject coin)
    {
        //reset when coin is retreived from the pool
        coin.SetActive(true);
    }

    /*
     * Called everytime a coin is returned to the pool. Calls OnRelease() on the coin to zero out the velocity so it doesn't carry momentum on its next spawn
     */

    private void OnReleaseCoin(GameObject coin)
    {
        coin.GetComponent<Coin>().OnRelease();
        coin.SetActive(false);
    }


    /*
     * 
     * Called when pool exceeds maxSize and needs to discard a coin entirely
     */
    private void OnDestroyCoin(GameObject coin)
    {
        if (Application.isPlaying)
        {
            Destroy(coin);
        }
        else
        {
            DestroyImmediate(coin);
        }
    }

    /*
     * The method that others cripts call to spawn a coin. Retreives a coin from the pool (triggers OnGetCoin), 
     * positions it, then calls OnSpawn which handles the flinging of the coin and starts the despawn timer.
     */

    public GameObject Get(Vector3 position)
    {
        var coinObject = pool.Get();
        var coin = coinObject.GetComponent<Coin>();
        coinObject.transform.position = position;
        coin.OnSpawn(flingForce);
        return coinObject;
    }

    /*
     * Get() -> OnGetCoin() -> OnSpawn() -> Delay -> OnReleaseCoin
     */
}
