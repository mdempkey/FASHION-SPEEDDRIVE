using UnityEngine;
using TMPro;

public class PersistentStyleManager : MonoBehaviour
{
    public static PersistentStyleManager Instance;

    public int stylePoints = 0;

    private TMP_Text stylePointsText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // destroy duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Register UI from scene-specific manager
    public void RegisterSceneUI(TMP_Text text)
    {
        stylePointsText = text;
        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        stylePoints += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (stylePointsText != null)
            stylePointsText.text = stylePoints.ToString();
    }

    public void ResetPoints()
    {
        stylePoints = 0;
        UpdateUI();
    }
}