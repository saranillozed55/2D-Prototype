using System;
using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour, IInteractable
{
    public static event Action OnDoubleJumpPickup;
    public void Interact()
    {
        if (!PlayerAbilityFlags.Instance.CheckAbilityFlag(Flags.HAS_DOUBLEJUMP))
        {
            PlayerAbilityFlags.Instance.SetAbilityFlag(Flags.HAS_DOUBLEJUMP, true);
            OnDoubleJumpPickup?.Invoke();

            Debug.Log(Flags.HAS_DOUBLEJUMP);
            //send event or do it here
            //add animations and stuff
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Already has Double Jump");
        }
    }
}
