using UnityEngine;

public class DialogueMenuCommand : ICommand
{

    public void Execute()
    {
        InputManager.SwitchMap(ActionMapType.UI);
        GameManager.Instance.SetCursorState(true);
    }
}
