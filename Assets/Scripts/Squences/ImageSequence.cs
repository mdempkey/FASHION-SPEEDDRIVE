using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageSequence : MonoBehaviour
{
    public RawImage canvasImage;
    public Texture firstImage;
    public Texture secondImage;
    public GameObject firstCanvas;
    public GameObject nextCanvas;
    public float fadeDuration = 1f;
    public float displayTime = 5f;

    private CanvasGroup cg;

    void Start()
    {
        // Get or add CanvasGroup for fading
        cg = firstCanvas.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = firstCanvas.AddComponent<CanvasGroup>();
        }

        StartCoroutine(ShowImageSequence());
    }

    IEnumerator ShowImageSequence()
    {
        firstCanvas.SetActive(true);
        nextCanvas.SetActive(false);

        // Show first image
        canvasImage.texture = firstImage;
        canvasImage.gameObject.SetActive(true);

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut());

        // Show second image
        canvasImage.texture = secondImage;

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut());

        // Switch to next canvas
        firstCanvas.SetActive(false);
        nextCanvas.SetActive(true);
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
    }
}