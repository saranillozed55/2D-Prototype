using UnityEngine;

public class DisableAxe : MonoBehaviour
{
    private SpriteRenderer axeSpriteRenderer;

    private void Awake()
    {
        axeSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DisableAxeSprite()
    {
        axeSpriteRenderer.enabled = false;
    }
}
