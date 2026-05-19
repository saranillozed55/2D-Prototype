using UnityEngine;
using UnityEngine.UIElements;

public class MenuEvents : MonoBehaviour
{
    public UIDocument document;
    private Button playButton;
    private bool isInitialized = false;


    //private void Awake()
    //{
    //    document = GetComponent<UIDocument>();
    //    playButton = document.rootVisualElement.Q("PlayButton") as Button;
    //    playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
    //}

    public void InitializeMenu()
    {
        if (isInitialized) return;
        if(document == null) document = GetComponent<UIDocument>();

        var root = document.rootVisualElement;

        if(root != null)
        {
            playButton = root.Q<Button>("PlayButton");
            if (playButton != null)
            {
                playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
                isInitialized = true;
                Debug.Log("Menu UI Initialized Successfully");
            }
        }
    }

    private void OnDisable()
    {
        if (playButton != null)
        {
            playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
            isInitialized = false;
        }
    }

    private void OnPlayButtonClicked(ClickEvent evt)
    {
        Debug.Log("Play Button Clicked");
        GameManager.Instance.UpdateGameState(GameState.Playing);
        document.enabled = false;
    }
}
