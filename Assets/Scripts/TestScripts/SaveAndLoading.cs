using UnityEngine;

public class SaveAndLoading : MonoBehaviour
{


    private void Awake()
    {
        SaveObject saveObject = new SaveObject { numCoins = 5 };

        string json = JsonUtility.ToJson(saveObject);
        Debug.Log(json);

        SaveObject loadedSaveObject = JsonUtility.FromJson<SaveObject>(json);
        Debug.Log(loadedSaveObject.numCoins);
    }
}

public class SaveObject
{
    public int numCoins;
}
