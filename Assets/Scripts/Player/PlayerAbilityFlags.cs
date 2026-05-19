using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerAbilityFlags : MonoBehaviour
{
    public static PlayerAbilityFlags Instance { get; private set; }

    private Dictionary<string, bool> abilityFlags = new Dictionary<string, bool>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keep alive between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetAbilityFlag(string flagName, bool value)
    {
        abilityFlags[flagName] = value;
    }

    public bool CheckAbilityFlag(string flagName)
    {
        //use val to make sure the flag is both found and set to true
        return abilityFlags.TryGetValue(flagName, out bool val) && val;
    }
}
