using System.Collections.Generic;
using UnityEngine;

public class WorldStateManager : GenericSingleton<WorldStateManager>
{
    //Stores all game events ("FirstBoss_Defeated" -> True)
    //Blank notebook where we can write down any important event that happens in the game, and whether it's true or false.
    private Dictionary<string, bool> worldFlags = new Dictionary<string, bool>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetFlag(string flagName, bool value)     
    {
        worldFlags[flagName] = value;
    }

    public bool CheckFlag(string flagName)
    {
        //1. Try to find the flag
        //2. If found, 'val' becomes whatever is in the dictionary(True or False)
        //3. We use && val to make sure the flag is both found and set to true;
        return worldFlags.TryGetValue(flagName, out bool val) && val;
    }
}
