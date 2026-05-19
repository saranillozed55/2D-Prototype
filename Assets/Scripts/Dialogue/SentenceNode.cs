using UnityEngine;

[CreateAssetMenu(fileName = "SentenceNode", menuName = "Dialogue/SentenceNode")]
public class SentenceNode : DialogueNode
{
    public string characterName;
    [TextArea] public string text;
}
