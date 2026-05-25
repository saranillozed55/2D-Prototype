using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField] private float _detectionRadius = 2f;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private bool _showGizmos;
    [SerializeField] private bool _showDetectionRadius = true;

    private Collider2D[] colliders;

    public override void Detect(AIData aiData)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _layerMask);
        aiData.obstacles = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (_showGizmos == false) 
        {
            return;
        }

        if(_showDetectionRadius)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }

        if(Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach(Collider2D obstacleCollider in colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
            }
        }
    }
}
