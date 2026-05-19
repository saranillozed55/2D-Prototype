using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : GenericSingleton<GameManager>
{
    public GameState currentState { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    private PlayerInputHandler playerInputHandler;

    private Dictionary<GameState, Action> stateHandlers;

    protected override void Awake()
    {
        base.Awake();
        stateHandlers = new Dictionary<GameState, Action> {
            {GameState.Playing, HandlePlaying},
            {GameState.Paused, HandlePaused },
            {GameState.Shopping, HandleShopping },
            {GameState.Dialogue, HandleDialogue },
        };

        playerInputHandler = FindFirstObjectByType<PlayerInputHandler>();
    }

    private void Start()
    {
        UpdateGameState(GameState.Playing);
    }

    public void UpdateGameState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if(stateHandlers.TryGetValue(newState, out var handler))
        {
            handler.Invoke();
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleMainMenu()
    {
        //This should be for going back to the mainmenu so it doesn't really do much right now
    }
    private void HandlePlaying()
    {
        Time.timeScale = 1.0f;
        playerInputHandler.RemoveLock(this);
        InputManager.SwitchMap(ActionMapType.Player);
        SetCursorState(false);
    }
    private void HandlePaused()
    {
        Time.timeScale = 0.0f;
        playerInputHandler.AddLock(this);
        InputManager.SwitchMap(ActionMapType.UI);
        SetCursorState(true);
    }

    private void HandleShopping()
    {
        Time.timeScale = 1.0f;
        playerInputHandler?.AddLock(this);
        InputManager.SwitchMap(ActionMapType.UI);
        SetCursorState(true);
    }

    private void HandleDialogue()
    {
        playerInputHandler.AddLock(this);
        ICommand command = new DialogueMenuCommand();
        command.Execute();
    }

    public void SetCursorState(bool UIActive)
    {
        Cursor.lockState = UIActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = UIActive;
    }
}
