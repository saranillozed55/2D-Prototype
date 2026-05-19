using cherrydev;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : GenericSingleton<DialogueManager>
{
    [SerializeField] private DialogBehaviour dialogueBehaviour;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        dialogueBehaviour.OnDialogStarted.AddListener(HandleDialogueStarted);
        dialogueBehaviour.OnDialogFinished.AddListener(HandleDialogueFinished);
    }
    private void OnDisable() {
        dialogueBehaviour.OnDialogStarted.RemoveListener(HandleDialogueStarted);
        dialogueBehaviour.OnDialogFinished.RemoveListener(HandleDialogueFinished);
    }


    public void StartDialogue(DialogNodeGraph nodeGraph)
    {
        dialogueBehaviour.StartDialog(nodeGraph);
    }

    private void HandleDialogueStarted()
    {
        GameManager.Instance.UpdateGameState(GameState.Dialogue);
    }

    private void HandleDialogueFinished()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
}
