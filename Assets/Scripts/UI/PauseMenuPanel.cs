using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuPanel : UIToolkitPanel
{
    private Button _playButton;
    private Button _settingsButton;

    protected override void Awake()
    {
        base.Awake();
        Root.AddToClassList("hidden");
        Debug.Log($"Pause Menu Panel was added to hidden: {Root.ClassListContains("hidden")}");
    }

    private void OnEnable() => PlayerInputHandler.OnMenu += HandleMenuPressed;
    private void OnDisable()
    {
        PlayerInputHandler.OnMenu -= HandleMenuPressed;
        if (_playButton != null) _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
        if (_settingsButton != null) _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        GameManager.Instance.UpdateGameState(GameState.Paused);
        if(Root != null)
        {
            _playButton = Root.Q<Button>("PlayButton");
            _settingsButton = Root.Q<Button>("SettingsButton");
            if(_playButton != null)
            {
                _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
                _settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
            }
        }
    }

    private void OnPlayButtonClicked(ClickEvent evt) 
    {
        Debug.Log("Play Button Clicked");
        UIStackManager.Instance.Pop();
    }

    private void OnSettingsButtonClicked(ClickEvent evt)
    {
        Debug.Log("Settings Button Clicked");
        //ADD: Settings Menu
    }

    private void HandleMenuPressed() => UIStackManager.Instance.Push(this);
}
