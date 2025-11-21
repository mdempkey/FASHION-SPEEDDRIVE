using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopcornFadeAnimator_Fixed : MonoBehaviour
{
    [Header("Timing Settings")]
    public float startDelay = 0f;       // Wait before starting the whole animation
    public float minDelay = 0.05f;      // Minimum delay between each image start
    public float maxDelay = 0.3f;       // Maximum delay between each image start
    public float fadeDuration = 0.5f;   // Time for the fade animation
    public float popScale = 1.15f;      // How much the image pops (1 = none)
    public float popDuration = 0.3f;    // (unused for now, keep for future)

    [Header("Options")]
    public bool randomizeSpritesInGrid = false; // If true, shuffle sprites between slots before animating
    public bool randomizeAnimationOrder = true; // If true, animation order is randomized (should be)

    private List<Image> images = new List<Image>();

    void Start()
    {
        // Gather only direct-child Images (safe)
        images.Clear();
        foreach (Transform child in transform)
        {
            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                images.Add(img);

                // Start transparent and slightly smaller visually (doesn't change layout)
                Color c = img.color;
                c.a = 0;
                img.color = c;
                img.transform.localScale = Vector3.one * 0.92f; // subtle small start
            }
        }

        // Optionally shuffle sprites between the slots (visual shuffle)
        if (randomizeSpritesInGrid)
            ShuffleSpritesBetweenSlots();

        // Shuffle animation order
        if (randomizeAnimationOrder)
            FisherYatesShuffle(images);

        StartCoroutine(AnimateImages());
    }

    // Fisher-Yates shuffle for the list (correct uniform shuffle)
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

    // Shuffle sprites between image slots (keeps each child in place but swaps sprites)
    void ShuffleSpritesBetweenSlots()
    {
        if (images.Count < 2) return;

        // Extract sprites
        List<Sprite> sprites = new List<Sprite>();
        foreach (var img in images)
            sprites.Add(img.sprite);

        // Shuffle sprite list
        FisherYatesShuffle(sprites);

        // Reassign shuffled sprites back to images
        for (int i = 0; i < images.Count; i++)
            images[i].sprite = sprites[i];
    }

    IEnumerator AnimateImages()
    {
        yield return new WaitForSeconds(startDelay);

        // If we want a non-blocking visual effect, start coroutines with staggered starts
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

            // Fade in alpha
            c.a = Mathf.Lerp(0, 1, Mathf.SmoothStep(0,1,tNorm));
            img.color = c;

            // Slight scale up (subtle pop) — stays close to 1 so layout won't change much
            t.localScale = Vector3.Lerp(startScale, targetScale, Mathf.SmoothStep(0, 1, tNorm));

            yield return null;
        }

        // Finalize to normal scale
        c.a = 1;
        img.color = c;
        t.localScale = Vector3.one;
    }
}
