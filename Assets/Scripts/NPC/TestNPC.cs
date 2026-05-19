using cherrydev;
using UnityEngine;

public class TestNPC : BaseNPC
{
    protected override void SendDialogue()
    {
        string keyToPlay = "GreetPlayer";

        if (WorldStateManager.Instance.CheckFlag(Flags.FIRST_BOSS))
        {
            keyToPlay = "PostBoss";
        }
        else if(WorldStateManager.Instance.CheckFlag(Flags.MET_NPC_ONCE))
        {
            keyToPlay = "ShortGreeting";
        }

        StartDialogueByKey(keyToPlay);

        //Checks in the dictionary if 'Met_NPC_Once' exists 
        // Realizes it doesn't exists so it creates the entry 'Met_NPC_Once' and sets its value to true
        WorldStateManager.Instance.SetFlag(Flags.MET_NPC_ONCE, true);
    }
}
