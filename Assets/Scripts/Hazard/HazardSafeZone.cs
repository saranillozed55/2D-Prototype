using Unity.Cinemachine;
using UnityEngine;

public class HazardSafeZone : MonoBehaviour
{
    private Transform respawnAnchor;
    private int playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        respawnAnchor = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == playerLayer)
        {
            Vector2 position = respawnAnchor.position;
            collider.GetComponent<PlayerHazardManager>().UpdateSafeSpot(position);
        }
    }
}
