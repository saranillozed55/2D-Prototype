using UnityEngine;

public class ContactTrigger : MonoBehaviour
{

    private NecromancerBoss _boss;

    private void Awake()
    {
        _boss = GetComponentInParent<NecromancerBoss>();
        if (_boss == null)
        {
            Debug.LogError("ContactTrigger could not find NecromancerBoss in parent hierarchy.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _boss.OnContactDamTrigger(collision);
    }

}
