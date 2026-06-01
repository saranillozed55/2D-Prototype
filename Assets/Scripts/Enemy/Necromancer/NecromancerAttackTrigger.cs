using UnityEngine;

public class NecromancerAttackTrigger : MonoBehaviour
{
    private NecromancerBoss _boss;

    private void Awake()
    {
        _boss = GetComponentInParent<NecromancerBoss>();
        if (_boss == null)
        {
            Debug.LogError("NecromancerAttackTrigger could not find NecromancerBoss in parent hierarchy.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _boss.OnAttackHit(collision);
    }
}
