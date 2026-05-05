using UnityEngine;

public class ScenarioSegmentVisual : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer[] backgroundRenderers;
    [SerializeField] private SpriteRenderer groundRenderer;
    [SerializeField] private SpriteRenderer ceilingRenderer;

    [Header("Tamaño del background")]
    [SerializeField] private float backgroundWidth = 30f;
    [SerializeField] private float backgroundHeight = 7f;

    public void Setup(Sprite backgroundSprite, Sprite groundSprite, Sprite ceilingSprite)
    {
        if (backgroundRenderers != null)
        {
            for (int i = 0; i < backgroundRenderers.Length; i++)
            {
                if (backgroundRenderers[i] == null) continue;

                backgroundRenderers[i].sprite = backgroundSprite;
                FitSpriteToSize(backgroundRenderers[i], backgroundWidth, backgroundHeight);
            }
        }

        if (groundRenderer != null)
        {
            groundRenderer.sprite = groundSprite;
        }

        if (ceilingRenderer != null)
        {
            ceilingRenderer.sprite = ceilingSprite;
        }
    }

    private void FitSpriteToSize(SpriteRenderer spriteRenderer, float targetWidth, float targetHeight)
    {
        if (spriteRenderer.sprite == null) return;

        spriteRenderer.transform.localScale = Vector3.one;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        float scaleX = targetWidth / spriteWidth;
        float scaleY = targetHeight / spriteHeight;

        spriteRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}