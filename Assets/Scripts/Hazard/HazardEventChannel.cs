using System;
using UnityEngine;

[CreateAssetMenu]
public class HazardEventChannel : ScriptableObject
{
    public event Action<GameObject, GameObject> OnHazardTriggered;

    public void Raise(GameObject source, GameObject target)
    {
        OnHazardTriggered?.Invoke(source, target);
    }
}
