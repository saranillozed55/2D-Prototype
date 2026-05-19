using System;
using UnityEngine;

public static class DialogueEvents
{
    public static event Action<DialogueGraph> OnSendDialogue;

    public static void SendDialogueGraph(DialogueGraph graph)
    {
        OnSendDialogue?.Invoke(graph);
    }
}
