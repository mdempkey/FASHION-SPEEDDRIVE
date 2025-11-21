using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopcornFadeAnimator : MonoBehaviour
{
    public float startDelay = 0f;
    public float minDelay = 0.05f;
    public float maxDelay = 0.3f;
    public float fadeDuration = 0.5f;
    public float popScale = 1.15f;
    public bool randomizeSpritesInGrid = false;
    public bool randomizeAnimationOrder = true;

    private List<Image> images = new List<Image>();

    void Start()
    {
        // Collect all child images
        images.Clear();
        foreach (Transform child in transform)
        {
            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                images.Add(img);

                // Start transparent and small
                Color c = img.color;
                c.a = 0;
                img.color = c;
                img.transform.localScale = Vector3.one * 0.92f;
            }
        }

        // Shuffle sprites if enabled
        if (randomizeSpritesInGrid)
            ShuffleSpritesBetweenSlots();

        // Shuffle animation order
        if (randomizeAnimationOrder)
            FisherYatesShuffle(images);

        StartCoroutine(AnimateImages());
    }

    void FisherYatesShuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }

    void ShuffleSpritesBetweenSlots()
    {
        if (images.Count < 2) return;

        // Get all sprites
        List<Sprite> sprites = new List<Sprite>();
        foreach (var img in images)
            sprites.Add(img.sprite);

        // Shuffle and reassign
        FisherYatesShuffle(sprites);
        for (int i = 0; i < images.Count; i++)
            images[i].sprite = sprites[i];
    }

    IEnumerator AnimateImages()
    {
        yield return new WaitForSeconds(startDelay);

        // Fade each image with random delay
        foreach (Image img in images)
        {
            StartCoroutine(FadeAndPop(img));
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    IEnumerator FadeAndPop(Image img)
    {
        float elapsed = 0f;
        Color c = img.color;
        Transform t = img.transform;

        Vector3 startScale = Vector3.one * 0.92f;
        Vector3 targetScale = Vector3.one * popScale;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float tNorm = Mathf.Clamp01(elapsed / fadeDuration);

            // Fade in
            c.a = Mathf.Lerp(0, 1, Mathf.SmoothStep(0, 1, tNorm));
            img.color = c;

            // Scale up slightly
            t.localScale = Vector3.Lerp(startScale, targetScale, Mathf.SmoothStep(0, 1, tNorm));

            yield return null;
        }

        // Set to final state
        c.a = 1;
        img.color = c;
        t.localScale = Vector3.one;
    }
}