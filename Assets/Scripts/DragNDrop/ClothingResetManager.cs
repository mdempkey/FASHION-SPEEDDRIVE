using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClothingResetManager : MonoBehaviour
{
    public Button resetButton;
    public Transform wardrobePanel;
    public StyleManager styleManager;
    
    // Store original item data by name
    private Dictionary<string, ItemData> originalStates = new Dictionary<string, ItemData>();
    
    private struct ItemData
    {
        public Vector2 anchoredPosition;
        public Quaternion rotation;
        public Vector3 scale;
        public Transform originalParent;
        public int siblingIndex;
    }
    
    void Start()
    {
        if (wardrobePanel == null)
        {
            Debug.LogError("WardrobePanel not assigned");
            return;
        }
        
        // Store initial states after a frame
        Invoke(nameof(StoreInitialStates), 0.1f);
        
        // Connect reset button
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetAllItems);
        }
    }
    
    void StoreInitialStates()
    {
        // Find all items in wardrobe
        DraggableItem[] allDragItems = wardrobePanel.GetComponentsInChildren<DraggableItem>();
        
        foreach (DraggableItem item in allDragItems)
        {
            RectTransform rectTransform = item.GetComponent<RectTransform>();
            
            if (rectTransform != null)
            {
                // Save item's starting position and parent
                ItemData data = new ItemData
                {
                    anchoredPosition = rectTransform.anchoredPosition,
                    rotation = rectTransform.rotation,
                    scale = rectTransform.localScale,
                    originalParent = rectTransform.parent,
                    siblingIndex = rectTransform.GetSiblingIndex()
                };
                
                originalStates[item.name] = data;
            }
        }
    }
    
    public void ResetAllItems()
    {
        DraggableItem[] currentItems = FindObjectsOfType<DraggableItem>();
        
        foreach (DraggableItem item in currentItems)
        {
            // Only reset items that started in wardrobe
            if (originalStates.ContainsKey(item.name))
            {
                ItemData data = originalStates[item.name];
                RectTransform rectTransform = item.GetComponent<RectTransform>();
                
                if (rectTransform != null && data.originalParent != null)
                {
                    // Move item back to original parent and position
                    rectTransform.SetParent(data.originalParent, false);
                    rectTransform.anchoredPosition = data.anchoredPosition;
                    rectTransform.localPosition = new Vector3(data.anchoredPosition.x, data.anchoredPosition.y, 0);
                    rectTransform.rotation = data.rotation;
                    rectTransform.localScale = data.scale;
                    rectTransform.SetSiblingIndex(data.siblingIndex);
                    
                    // Reset visibility
                    CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = 1f;
                        canvasGroup.blocksRaycasts = true;
                    }
                }
            }
        }
    }
}