using System.Collections;
using UnityEngine;

/*
 *Later implement localtimescale for some enemies
 */
public class HitStop : GenericSingleton<HitStop>
{
    private bool isWaiting = false;

    protected override void Awake()
    {
        base.Awake();
    }
    public void Stop(float duration)
    {
        if (isWaiting) return;
        StopAllCoroutines(); // stop's hitstops from stacking
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
