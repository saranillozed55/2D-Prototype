using UnityEngine;

public enum BossDirection { Left, Right };
public class BossRoomTrigger : MonoBehaviour
{
    [Header("Direction of Boss")]
    [SerializeField] private BossDirection _bossDirection;
    [SerializeField] private BoxCollider2D _collider;

    [Header("Broadcast Event Channels")]
    [SerializeField] private VoidEventChannelSO _playerWalkedIntoBoss;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _collider.bounds.center).normalized;

            if (exitDirection.x > 0 && _bossDirection == BossDirection.Right)
            {
                Debug.Log("Player walked of trigger towards right side");
                _playerWalkedIntoBoss.RaiseEvent();
            }
            if (exitDirection.x < 0 && _bossDirection == BossDirection.Left)
            {
                Debug.Log("Player walked of trigger towards left side");
                _playerWalkedIntoBoss.RaiseEvent();
            }
        }
    }
}