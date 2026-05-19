using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnswerNode", menuName = "Dialogue/Answer Node")]
public class AnswerNode : DialogueNode
{
    public List<DialogueAnswer> answers;
    
}

[Serializable]
public class  DialogueAnswer 
{
    public string answerText;
    public DialogueNode nextNode;
}
