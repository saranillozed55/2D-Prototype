//using System.Collections;
//using Unity.VisualScripting;
//using UnityEngine;

//public class EnemyVisuals : MonoBehaviour
//{
//    private TestEnemy enemy;
//    private Breakable breakable;


//    [SerializeField] private float deathTimer = 1f;

//    private void Awake()
//    {
//        enemy = GetComponent<TestEnemy>();
//        breakable = GetComponent<Breakable>();
//    }

//    private void OnEnable()
//    {
//        if (enemy != null)
//        {
//            breakable.OnDeath += HandleDeathVisuals;
//            enemy.OnSpottedPlayer += HandleOnSpottedPlayer;
//        }
//    }
//    private void OnDisable()
//    {
//        if (enemy != null)
//        {
//            breakable.OnDeath -= HandleDeathVisuals;
//            enemy.OnSpottedPlayer -= HandleOnSpottedPlayer;
//        }
//    }

//    private void HandleDeathVisuals()
//    {
//        StartCoroutine(DeathRoutine());
//    }

//    private void HandleOnSpottedPlayer()
//    {

//    }

//    private IEnumerator DeathRoutine()
//    {
//        //Handle Visuals animations

//        yield return new WaitForSeconds(deathTimer);

//        Destroy(gameObject);
//    }
//}
