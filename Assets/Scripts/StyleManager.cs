using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class StyleManager : MonoBehaviour
{
    public int stylePoints = 0;
    public TMP_Text stylePointsText;

    [Header("Current Outfit")]
    public string top;
    public string pants;
    public string shoes;
    public string accessory;

    private int lastPoints = 0;
    private Coroutine popCoroutine;

    void Start()
    {
        UpdateStylePointsUI();
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

        if (top == "disco_jacket" && pants == "disco_pants" && shoes == "disco_shoes" && accessory == "disco_glasses")
            score += 30;

        stylePoints = score;
        UpdateStylePointsUI();
    }

    void UpdateStylePointsUI()
    {
        if (stylePointsText == null)
        {
            Debug.LogWarning("StyleManager: TMP text not assigned!");
            return;
        }

        stylePointsText.text = stylePoints.ToString();

        // Only animate if value changed
        if (stylePoints != lastPoints)
        {
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

        // Scale up
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            stylePointsText.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // Scale back
        time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            stylePointsText.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
    }
}
