using System;
using UnityEngine;

public abstract class GenericEventChannelSO<T> : ScriptableObject
{
    public event Action<T> OnEventRaised;

    public void RaiseEvent(T parameter)
    {
        OnEventRaised?.Invoke(parameter);
    }
}
