using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StyleManager : MonoBehaviour
{
    public int stylePoints = 0;
    public TMP_Text stylePointsText;  

    [Header("Current Outfit")]
    public string top;
    public string pants;
    public string shoes;
    public string accessory;

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

        if (top == "pink_jacket" && pants == "sparkle_skirt")
            score += 50;
        if (shoes == "heels_glitter")
            score += 20;
        if (accessory == "heart_bag")
            score += 10;

// extra bouns
        if (top == "pink_jacket" && pants == "sparkle_skirt" && shoes == "heels_glitter" && accessory == "heart_bag")
            score += 30;

        stylePoints = score;
        UpdateStylePointsUI();
    }

    void UpdateStylePointsUI()
    {
        if (stylePointsText != null)
            stylePointsText.text = "Style Points: " + stylePoints;
    }
}