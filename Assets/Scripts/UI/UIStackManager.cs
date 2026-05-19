using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UIStackManager : GenericSingleton<UIStackManager>
{
    [SerializeField] private PlayerInputHandler playerInputHandler;

    private readonly Stack<IUIPanel> _panelStack = new();

    public void RegisterInputHandler(PlayerInputHandler handler)
    {
        playerInputHandler = handler;
    }

    public void Push(IUIPanel panel)
    {
        if(_panelStack.Count == 0)
        {
            playerInputHandler.AddLock(this);
        }
        if (_panelStack.Count > 0)
        {
            _panelStack.Peek().OnLostFocus();
        }

        _panelStack.Push(panel);
        panel.OnOpen();
    }

    public void Pop()
    {
        if (_panelStack.Count == 0) return;
        
        _panelStack.Pop().OnClose();

        if(_panelStack.Count > 0)
        {
            _panelStack.Peek().OnGainedFocus();
        }
        else
        {
            OnStackEmpty();
        }
    }

    public void PopAll()
    {
        while(_panelStack.Count > 0)
        {
            _panelStack.Pop().OnClose();
        }
        OnStackEmpty();
    }

    private void OnStackEmpty()
    {
        playerInputHandler.RemoveLock(this);
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
}
