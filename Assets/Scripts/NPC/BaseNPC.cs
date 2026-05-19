using cherrydev;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNPC : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public struct DialogueEntry {
        public string dialogueKey;
        public DialogNodeGraph nodeGraph;
    }

    //[System.Serializable]
    //public struct DialogueEntries 
    //{
    //    public string dialogueKey;
    //    public DialogueGraph nodeGraph;
    //}
    

    [Header("Dialogue Database")]
    [SerializeField] protected List<DialogueEntry> dialogueDatabase;

    //[Header("Dialogue Data")]
    //[SerializeField] protected List<DialogueEntries> dialogueData;

    protected Dictionary<string, DialogNodeGraph> dialogueLookup = new Dictionary<string, DialogNodeGraph>();

    protected bool canInteract = true;

    protected void Awake()
    {
        //Convert the inespector list into a dictionary for quick access
        foreach (var entry in dialogueDatabase)
        {
            if(!string.IsNullOrEmpty(entry.dialogueKey) && !dialogueLookup.ContainsKey(entry.dialogueKey))
                dialogueLookup.Add(entry.dialogueKey, entry.nodeGraph);
        }
    }

    public virtual void Interact()
    {
        if (!canInteract) return;
        SendDialogue();
    }
    protected virtual void SendDialogue()
    {
        //Override this method by specific NPCs
    }

    protected void StartDialogueByKey(string key)
    {
        //'targetGraph' is created right here. If the key exists, targetGraph is filled with the data
        if(dialogueLookup.TryGetValue(key, out DialogNodeGraph targetGraph))
        {
            //Inside these brackets, targetGraph is safe to use and contains the dialogue data we want to start
            DialogueManager.Instance.StartDialogue(targetGraph);
        }
        else
        {
            Debug.LogWarning($"Dialogue key {key} not found in NPC {name}'s dialogue database.");

        }
    }
}
