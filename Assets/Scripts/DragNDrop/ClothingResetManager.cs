using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClothingResetManager : MonoBehaviour
{
    [Header("Reset Button")]
    public Button resetButton;
    
    [Header("Wardrobe Panel (Where items start)")]
    public Transform wardrobePanel;
    
    [Header("Optional: Style Manager")]
    public StyleManager styleManager;
    
    // Store original positions by item name instead of GameObject reference
    private Dictionary<string, ItemData> originalStates = new Dictionary<string, ItemData>();
    
    private struct ItemData
    {
        public string itemName;
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
            Debug.LogError("⚠️ WardrobePanel not assigned! Please assign it in the Inspector.");
            return;
        }
        
        // Wait a frame to ensure all items are initialized
        Invoke(nameof(StoreInitialStates), 0.1f);
        
        // Hook up reset button
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetAllItems);
        }
        else
        {
            Debug.LogWarning("Reset button not assigned in ClothingResetManager!");
        }
    }
    
    void StoreInitialStates()
    {
        // Only store items that are children of the wardrobePanel
        DraggableItem[] allDragItems = wardrobePanel.GetComponentsInChildren<DraggableItem>();
        
        foreach (DraggableItem item in allDragItems)
        {
            GameObject obj = item.gameObject;
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            
            if (rectTransform != null)
            {
                ItemData data = new ItemData
                {
                    itemName = obj.name,
                    anchoredPosition = rectTransform.anchoredPosition,
                    rotation = rectTransform.rotation,
                    scale = rectTransform.localScale,
                    originalParent = rectTransform.parent,
                    siblingIndex = rectTransform.GetSiblingIndex()
                };
                
                // Use the GameObject name as the key
                originalStates[obj.name] = data;
                Debug.Log($"Stored: {obj.name} at position {data.anchoredPosition}");
            }
        }
        
        Debug.Log($"✅ Stored {originalStates.Count} items from {wardrobePanel.name}");
    }
    
    public void ResetAllItems()
    {
        Debug.Log("🔄 Resetting all wardrobe items...");
        
        // Find all draggable items currently in the scene
        DraggableItem[] currentItems = FindObjectsOfType<DraggableItem>();
        int resetCount = 0;
        
        foreach (DraggableItem item in currentItems)
        {
            GameObject obj = item.gameObject;
            
            // Check if we have stored data for this item (only wardrobe items)
            if (originalStates.ContainsKey(obj.name))
            {
                ItemData data = originalStates[obj.name];
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                
                if (rectTransform != null && data.originalParent != null)
                {
                    // Set parent back to original
                    rectTransform.SetParent(data.originalParent, false);
                    
                    // Restore position
                    rectTransform.anchoredPosition = data.anchoredPosition;
                    rectTransform.localPosition = new Vector3(data.anchoredPosition.x, data.anchoredPosition.y, 0);
                    rectTransform.rotation = data.rotation;
                    rectTransform.localScale = data.scale;
                    
                    // Restore order in hierarchy
                    rectTransform.SetSiblingIndex(data.siblingIndex);
                    
                    // Reset CanvasGroup to ensure visibility and interactivity
                    CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = 1f;
                        canvasGroup.blocksRaycasts = true;
                    }
                    
                    Debug.Log($"✅ Reset: {obj.name} back to {data.originalParent.name}");
                    resetCount++;
                }
            }
            // If item is not in our dictionary, it's not a wardrobe item (like TieShoes)
            // so we ignore it
        }
        
        // Optional: Clear the style manager's selections
        if (styleManager != null)
        {
            // This assumes StyleManager has a method to clear selections
            // styleManager.ClearAllSelections();
        }
        
        Debug.Log($"✅ Reset complete! {resetCount} items returned to wardrobe");
    }
}