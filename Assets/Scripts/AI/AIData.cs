using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    //Used for multiple targets
    public List<Transform> targets = null;

    //store all the obstacles around the enemy that it should avoid
    public Collider2D[] obstacles = null;

    public Transform currentTarget;

    public int GetTargestCount() => targets == null ? 0 : targets.Count;

}
