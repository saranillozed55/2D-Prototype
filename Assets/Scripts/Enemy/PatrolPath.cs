using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<Transform> wayPoints;

    private void OnDrawGizmosSelected()
    {
        if(wayPoints == null || wayPoints.Count == 0)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        for(int i = 0; i < wayPoints.Count; i++)
        {
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[(i + 1) % wayPoints.Count].position);
        }
    }
}
