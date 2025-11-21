using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Blinker : MonoBehaviour
{
    private Image image;
    public float fadeDuration = 0.5f; 
    public float pauseDuration = 0.5f; 

    void Start()
    {
        image = GetComponent<Image>();

        StartCoroutine(BlinkSequence());
    }

    IEnumerator BlinkSequence()
    {
        while (true) 
        {
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration)); 
            yield return new WaitForSeconds(pauseDuration);

            yield return StartCoroutine(Fade(0f, 1f, fadeDuration)); 

            yield return new WaitForSeconds(pauseDuration);
        }
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        Color color = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = color;

            yield return null; 
        }

        color.a = endAlpha;
        image.color = color;
    }
}