using System.Collections;
using UnityEngine;

public class PlayerHazardManager : MonoBehaviour
{
    private PlayerController controller;
    private Vector2 lastSafePosition;
    private bool isRespawning = false;

    [Header("Hazard Channel")]
    [SerializeField] private HazardEventChannel hazardChannel;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        hazardChannel.OnHazardTriggered += RespawnAtLastSafeSpot;
    }
    private void OnDisable()
    {
        hazardChannel.OnHazardTriggered -= RespawnAtLastSafeSpot;

    }

    public void UpdateSafeSpot(Vector2 position)
    {
        //only update if the distance between the old and new spot is more than a tiny bit
        // prevents "re-updating" the same spot constantly
        if (Vector2.Distance(lastSafePosition, position) > 0.1f)
        {
            Debug.Log("Updated Respawn position");
            lastSafePosition = position;
        }
    }

    public void RespawnAtLastSafeSpot(GameObject source, GameObject target)
    {
        if (isRespawning) return;
        StartCoroutine(HazardRespawnCoroutine());
    }

    private IEnumerator HazardRespawnCoroutine()
    {
        //play animations
        isRespawning = true;
        transform.position = lastSafePosition;
        
        yield return new WaitForSeconds(0.5f);
        isRespawning = false;
    }
}
