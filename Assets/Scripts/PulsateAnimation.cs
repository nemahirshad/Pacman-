using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PulsateAnimation : MonoBehaviour
{
    // === New Glowing Color Settings ===
    [Header("Glow Color & Duration")]
    [Tooltip("The color the sprite pulses towards for the glow effect.")]
    [SerializeField] private Color glowColor = Color.white;

    [Tooltip("How long (in seconds) the object should pulsate for before stopping.")]
    [SerializeField] private float duration = 5f;

    // === Pulsation Settings ===
    [Header("Pulsation Settings")]
    [Tooltip("The speed at which the object fades in and out.")]
    [SerializeField] private float pulseSpeed = 1f;

    [Tooltip("The minimum transparency (alpha) the object reaches.")]
    [SerializeField] private float minAlpha = 0.5f;

    [Tooltip("The maximum scale factor the object reaches (1.0 = no size change).")]
    [SerializeField] private float maxScale = 1.05f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private float startTime;

    private void Awake()
    {
        // Get the SpriteRenderer and store original values
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;

        // Start the timer when the object is initialized
        startTime = Time.time;
    }

    private void Update()
    {
        // 1. Check Duration Limit
        if (Time.time >= startTime + duration)
        {
            // Reset to original appearance and stop script execution
            spriteRenderer.color = originalColor;
            transform.localScale = originalScale;
            enabled = false;
            return;
        }

        // Use Time.time to create a smooth, looping value based on the Sine wave.
        // t maps the sine wave from 0 (min) to 1 (max)
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // === 2. Handle Color (Glowing/Fading) ===

        // Calculate the pulsed color by blending the original color with the glow color.
        // The closer t is to 1, the more the color shifts to the bright glowColor.
        Color pulsedColor = Color.Lerp(originalColor, glowColor, t);

        // Calculate the new alpha value, interpolating between minAlpha and maxAlpha
        float newAlpha = Mathf.Lerp(minAlpha, originalColor.a, t);

        // Apply the new alpha to the pulsed color
        pulsedColor.a = newAlpha;

        // Set the final color
        spriteRenderer.color = pulsedColor;

        // === 3. Handle Scale (Throbbing) ===

        // Calculate the new scale factor, interpolating between originalScale and maxScale
        float scaleFactor = Mathf.Lerp(1f, maxScale, t);

        // Apply the new scale to the object
        transform.localScale = originalScale * scaleFactor;
    }
}