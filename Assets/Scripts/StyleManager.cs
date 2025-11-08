using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // for scene loading
using System.Collections;

public class StyleManager : MonoBehaviour
{
    [Header("Points & UI")]
    public int stylePoints = 0;
    public TMP_Text stylePointsText;

    [Header("Sounds")]
    public AudioSource completeSound;
    public AudioSource pointSound;

    [Header("Current Outfit")]
    public string top;
    public string pants;
    public string shoes;
    public string accessory;

    [Header("Character Display")]
    public Image characterDisplay;
    public Sprite defaultCharacterSprite;
    public Sprite styledCharacterSprite;

    [Header("Slot Images (the ones that show the clothes)")]
    public Image topSlot;
    public Image pantsSlot;
    public Image shoesSlot;
    public Image accessorySlot;

    [Header("Scene Transition")]
    public string nextSceneName = "PartyScene"; // set in Inspector
    public float sceneDelay = 3f; // seconds before switching

    private int lastPoints = 0;
    private Coroutine popCoroutine;
    private bool outfitComplete = false;

    void Start()
    {
        UpdateStylePointsUI();

        if (characterDisplay != null && defaultCharacterSprite != null)
            characterDisplay.sprite = defaultCharacterSprite;
    }

    public void SelectOutfitPiece(string category, string itemName)
    {
        switch (category.ToLower())
        {
            case "top": top = itemName; break;
            case "pants": pants = itemName; break;
            case "shoes": shoes = itemName; break;
            case "accessory": accessory = itemName; break;
        }

        EvaluateOutfit();
    }

    void EvaluateOutfit()
    {
        int score = 0;

        if (top == "disco_jacket")
            score += 50;
        if (shoes == "disco_shoes")
            score += 20;
        if (accessory == "disco_glasses")
            score += 10;
        if (pants == "disco_pants")
            score += 40;

        bool isFullOutfit = (top == "disco_jacket" &&
                             pants == "disco_pants" &&
                             shoes == "disco_shoes" &&
                             accessory == "disco_glasses");

        if (isFullOutfit)
            score += 30;

        stylePoints = score;
        UpdateStylePointsUI();

        if (isFullOutfit && !outfitComplete)
        {
            ApplyStyledLook();
            outfitComplete = true;
        }
    }

    void ApplyStyledLook()
    {
        if (characterDisplay != null && styledCharacterSprite != null)
        {
            StartCoroutine(FadeCharacterSprite(styledCharacterSprite));
        }

        // Clear outfit data
        top = pants = shoes = accessory = "";

        // ✨ Clear the slot images
        ClearSlotImage(topSlot);
        ClearSlotImage(pantsSlot);
        ClearSlotImage(shoesSlot);
        ClearSlotImage(accessorySlot);

        Debug.Log("✨ Full outfit complete! Styled look applied and slots cleared!");
        
        // 🎵 Play success sound
        if (completeSound != null)
            completeSound.Play();

        // ⏳ Go to next scene after a short delay
        StartCoroutine(GoToNextSceneAfterDelay());
    }

    IEnumerator GoToNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(sceneDelay);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name not set in StyleManager!");
        }
    }

    void ClearSlotImage(Image slot)
    {
        if (slot != null)
        {
            StartCoroutine(FadeOutSlot(slot));
            slot.gameObject.SetActive(false); // hide the slot entirely
        }
    }

    IEnumerator FadeOutSlot(Image slot)
    {
        float duration = 0.3f;
        float time = 0f;
        Color color = slot.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, time / duration);
            slot.color = color;
            yield return null;
        }

        slot.sprite = null;
        color.a = 1;
        slot.color = color;
    }

    IEnumerator FadeCharacterSprite(Sprite newSprite)
    {
        float duration = 0.5f;
        float time = 0f;
        Image img = characterDisplay;
        Color color = img.color;

        // Fade out
        while (time < duration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, time / duration);
            img.color = color;
            yield return null;
        }

        img.sprite = newSprite;
        time = 0f;

        // Fade in
        while (time < duration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, time / duration);
            img.color = color;
            yield return null;
        }
    }

    void UpdateStylePointsUI()
    {
        if (stylePointsText == null)
        {
            Debug.LogWarning("StyleManager: TMP text not assigned!");
            return;
        }

        stylePointsText.text = stylePoints.ToString();

        if (stylePoints != lastPoints)
        {
            if (stylePoints > lastPoints && pointSound != null)
                pointSound.Play();  // 🎵 play sound when points increase

            if (popCoroutine != null)
                StopCoroutine(popCoroutine);
            popCoroutine = StartCoroutine(PopTextEffect());
        }

        lastPoints = stylePoints;
    }

    IEnumerator PopTextEffect()
    {
        float duration = 0.3f;
        float time = 0;
        Vector3 originalScale = stylePointsText.rectTransform.localScale;
        Vector3 targetScale = originalScale * 1.3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            stylePointsText.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, time / duration);
            yield return null;
        }

        time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            stylePointsText.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, time / duration);
            yield return null;
        }
    }
}
