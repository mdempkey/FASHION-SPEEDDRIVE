using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

public class Blinker : MonoBehaviour
{
    private Image image;
    public float fadeDuration = 0.5f; // How long one fade takes (in seconds)
    public float pauseDuration = 0.5f; // How long to stay fully visible/invisible

    void Start()
    {
        // Get the Image component attached to this GameObject
        image = GetComponent<Image>();

        // Start the continuous blinking routine
        StartCoroutine(BlinkSequence());
    }

    IEnumerator BlinkSequence()
    {
        while (true) // Infinite loop to keep blinking
        {
            // 1. FADE OUT (Pop Out)
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration)); // From fully visible (1) to invisible (0)

            // Pause while invisible
            yield return new WaitForSeconds(pauseDuration);

            // 2. FADE IN (Pop In)
            yield return StartCoroutine(Fade(0f, 1f, fadeDuration)); // From invisible (0) to fully visible (1)

            // Pause while visible
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    // Coroutine to handle the smooth transition of alpha
    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        // Get the current color to modify the alpha
        Color color = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate the alpha value (t = time from 0 to 1)
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = color;

            yield return null; // Wait until the next frame
        }

        // Ensure the alpha is exactly the end value when done
        color.a = endAlpha;
        image.color = color;
    }
}