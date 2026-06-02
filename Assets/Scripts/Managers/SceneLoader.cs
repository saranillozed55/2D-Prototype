using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : GenericSingleton<SceneLoader>
{
    [SerializeField] private float transitionTime = 1f;

    public static event Action OnSceneTransitionStart;
    public static event Action OnSceneTransitionEnd;

    public void LoadScene()
    {
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadNextScene(int index)
    {
        OnSceneTransitionStart?.Invoke(); // screen fader will listen to this event and start fading out
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(index);
        //OnSceneTransitionEnd?.Invoke();
    }

}
