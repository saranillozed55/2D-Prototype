using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    //Range that we can see player. Might not use this because we have BatController but can also change batcontroller
    [SerializeField] private float _targetDetectionRange = 5f;

    [SerializeField] private LayerMask _obstacleLayerMask, _playerLayerMask;

    [SerializeField] private bool _showGizmos = false;

    //gizmos params
    private List<Transform> _colliders;

    public override void Detect(AIData aiData)
    {

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _targetDetectionRange, _playerLayerMask);

        if (playerCollider != null)
        {
            //Check if we can see the player
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _targetDetectionRange, _obstacleLayerMask);

            // make sure that the collider we see is on the "Player" layer
            if (hit.collider != null && (_playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * _targetDetectionRange, Color.aquamarine);
                _colliders = new List<Transform>() { playerCollider.transform };
            }
            else
            {
                _colliders = null;
            }
        }
        else
        {
            _colliders = null;
        }
        aiData.targets = _colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (_showGizmos == false) return;
        Gizmos.DrawWireSphere(transform.position, _targetDetectionRange);

        if (_colliders == null) return;

        foreach(var item in _colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }

    }
}
