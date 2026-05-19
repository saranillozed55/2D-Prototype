using System.Collections;
using UnityEngine;

/*
 *Later implement localtimescale for some enemies
 */
public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    private bool isWaiting = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Stop(float duration)
    {
        if (isWaiting) return;
        StartCoroutine(Wait(duration));
    }

    private IEnumerator Wait(float duration)
    {
        isWaiting = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        isWaiting = false;
    }
}
