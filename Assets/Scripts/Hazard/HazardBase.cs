using System;
using System.Collections;
using UnityEditor.U2D.Animation;
using UnityEngine;

public abstract class HazardBase : MonoBehaviour
{
    public bool isTriggering;
    [SerializeField] private LayerMask hazardTargets;

    [SerializeField] private HazardEventChannel channel;

    private void OnValidate()
    {
        if(hazardTargets == 0)
        {
            hazardTargets = LayerMask.GetMask("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggering && ((1 << collision.gameObject.layer) & hazardTargets) != 0)
        {
            RaiseHazardTriggered(collision.gameObject);
            StartCoroutine(HazardCooldown());
        }
    }

    protected void RaiseHazardTriggered(GameObject target)
    {
        channel.Raise(gameObject,target);
        Debug.Log("Raised Hazard Event");
    }

    private IEnumerator HazardCooldown()
    {
        //prevent double triggering during respawn
        isTriggering = true;
        yield return new WaitForSeconds(0.5f);
        isTriggering = false;
    }
}
