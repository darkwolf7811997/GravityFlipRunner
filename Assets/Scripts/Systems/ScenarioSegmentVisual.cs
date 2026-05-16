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

    public void Setup(ScenarioManager.Scenario scenario, int segmentIndex)
    {
        if (scenario == null) return;

        ApplyBackgrounds(scenario.backgroundSprites, segmentIndex);

        if (groundRenderer != null)
            groundRenderer.sprite = scenario.groundSprite;

        if (ceilingRenderer != null)
            ceilingRenderer.sprite = scenario.ceilingSprite;
    }

    private void ApplyBackgrounds(Sprite[] sprites, int segmentIndex)
    {
        if (backgroundRenderers == null || backgroundRenderers.Length == 0) return;
        if (sprites == null || sprites.Length == 0) return;

        for (int i = 0; i < backgroundRenderers.Length; i++)
        {
            if (backgroundRenderers[i] == null) continue;

            int spriteIndex = (segmentIndex * backgroundRenderers.Length + i) % sprites.Length;

            backgroundRenderers[i].sprite = sprites[spriteIndex];

            FitSpriteToSize(backgroundRenderers[i], backgroundWidth, backgroundHeight);
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