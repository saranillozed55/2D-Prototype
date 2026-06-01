using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines.ExtrusionShapes;

public class SaveManager : GenericSingleton<SaveManager>
{
    private PlayerData playerData;
    private PlayerInputHandler playerInputHandler;


    protected override void Awake()
    {
        //For testing purposes: playerInputHandler
        playerInputHandler = FindFirstObjectByType<PlayerInputHandler>();

        base.Awake();
        CacheReferences();

        SaveSystem.Init();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerInputHandler.OnSave += Save;
        playerInputHandler.OnLoad += Load;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        playerInputHandler.OnSave -= Save;
        playerInputHandler.OnLoad -= Load;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CacheReferences();
    }

    private void CacheReferences()
    {
        playerData = FindFirstObjectByType<PlayerData>();
    }

    private void Save()
    {
        Vector3 playerPos = playerData.GetPlayerPosition();

        SavedObject savedObject = new SavedObject
        {
            playerPosition = playerPos,
        };

        string json = JsonUtility.ToJson(savedObject);
        SaveSystem.Save(json);
        Debug.Log($"Saved Data: {json}");
    }

    private void Load()
    {
        string saveString = SaveSystem.Load();
        if(saveString != null)
        {
            Debug.Log("Loaded:" + saveString);
            SavedObject saveObject = JsonUtility.FromJson<SavedObject>(saveString);

            playerData.SetPlayerPosition(saveObject.playerPosition);
        }

        else
        {
            Debug.Log("No Save was found.");
        }
    }

    //private void OnLevelWasLoaded(int level)
    //{

    //}
}

public class SavedObject
{
    public Vector3 playerPosition;
}
