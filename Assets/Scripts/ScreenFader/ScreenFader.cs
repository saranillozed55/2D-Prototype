using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenFader : MonoBehaviour
{
    [Header("Listener to Event Channels")]
    [SerializeField] private VoidEventChannelSO _onPlayerDeath;
    [SerializeField] private VoidEventChannelSO _onPlayerRespawn;

    [Header("References")]
    [SerializeField] private UIDocument _document;

    private VisualElement fadeElement;

    [Header("Settings")]
    [SerializeField] private float duration = 1.0f;

    private void Awake()
    {
        fadeElement = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
        fadeElement.AddToClassList("hidden");
    }

    private void OnEnable()
    {
        
        _onPlayerDeath.OnEventRaised += FadeToBlack;
        _onPlayerRespawn.OnEventRaised += FadeFromBlack;
    }
    private void OnDisable()
    {
        _onPlayerDeath.OnEventRaised -= FadeToBlack;
        _onPlayerRespawn.OnEventRaised -= FadeFromBlack;

    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeElement.style.backgroundColor.value;
        color.a = alpha;
        fadeElement.style.backgroundColor = new StyleColor(color);
    }

    public void FadeToBlack() 
    {
         StartCoroutine(Fade(0, 1));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Fade(1, 0));
    }
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        fadeElement.RemoveFromClassList("hidden");
        float elapsed = 0f;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, endAlpha, elapsed / duration));
            yield return null;
        }
        SetAlpha(endAlpha);
        if(endAlpha == 0f)
        {
            fadeElement.AddToClassList("hidden");
        }
    }
}
