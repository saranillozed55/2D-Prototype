using UnityEngine;

public abstract class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Properties:

    public static T Instance { get; private set; }
    #endregion


    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    ////without this, Instance still points to the destroyed object from last session
    //static void ResetStatics()
    //{
    //    Instance = null;
    //}

    #region MonoBehaviour Callback Method(s):
    protected virtual void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this as T;
        //DontDestroyOnLoad(gameObject);
    }
    #endregion
}
