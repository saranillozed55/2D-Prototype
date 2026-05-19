using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueGraph", menuName = "Dialogue/Dialogue Graph")]
public class DialogueGraph : ScriptableObject
{
    public DialogueNode startNode;
    public List<DialogueNode> allNodes;
}
