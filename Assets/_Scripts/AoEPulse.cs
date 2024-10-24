using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PulseColor : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private Color pulseColor = Color.red;    // The target color to pulse to
    [SerializeField] private float pulseSpeed = 2f;          // Speed of the pulsing effect
    [SerializeField] private float minAlpha = 0.3f;          // Minimum alpha value
    [SerializeField] private float maxAlpha = 0.7f;          // Maximum alpha value

    private SpriteRenderer spriteRenderer;
    private float alpha;
    private bool isIncreasing = true;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("PulseColor script requires a SpriteRenderer component.");
            enabled = false;
            return;
        }

        // Initialize alpha to the current color's alpha
        alpha = spriteRenderer.color.a;
    }

    void Update()
    {
        // Update the alpha value based on pulsing direction
        if (isIncreasing)
        {
            alpha += Time.deltaTime * pulseSpeed;
            if (alpha >= maxAlpha)
            {
                alpha = maxAlpha;
                isIncreasing = false;
            }
        }
        else
        {
            alpha -= Time.deltaTime * pulseSpeed;
            if (alpha <= minAlpha)
            {
                alpha = minAlpha;
                isIncreasing = true;
            }
        }

        // Apply the new alpha to the sprite's color
        Color newColor = pulseColor;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}
