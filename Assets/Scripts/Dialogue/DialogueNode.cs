using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/DialogueNode")]
public abstract class DialogueNode : ScriptableObject
{
    public DialogueNode nextNode;
}
