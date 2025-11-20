using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageSequence : MonoBehaviour
{
    public RawImage canvasImage;
    public Texture firstImage;
    public Texture secondImage;

    public GameObject firstCanvas;   // Dialogue canvas
    public GameObject nextCanvas;    // Canvas that appears AFTER

    public float fadeDuration = 1f;
    public float displayTime = 5f;   // Time each image stays fully visible

    private CanvasGroup cg;

    void Start()
    {
        cg = firstCanvas.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = firstCanvas.AddComponent<CanvasGroup>();
        }

        StartCoroutine(ShowImageSequence());
    }

    IEnumerator ShowImageSequence()
    {
        // Start with dialogue canvas
        firstCanvas.SetActive(true);
        nextCanvas.SetActive(false);

        // First image
        canvasImage.texture = firstImage;
        canvasImage.gameObject.SetActive(true);

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut());

        // Second image
        canvasImage.texture = secondImage;

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut());

        // Hide dialogue canvas
        firstCanvas.SetActive(false);

        // Show next canvas (no fade needed, unless you want it)
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
