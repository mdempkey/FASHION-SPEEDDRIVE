using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public string acceptedCategory;
    public StyleManager styleManager;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem == null) return;

        // Only accept if the category matches
        if (draggedItem.category.ToLower() == acceptedCategory.ToLower())
        {
            // Snaps item into this slot
            draggedItem.transform.SetParent(transform, true);
            draggedItem.transform.localPosition = Vector3.zero;

            // Tells StyleManager what was placed
            if (styleManager != null)
            {
                styleManager.SelectOutfitPiece(acceptedCategory, draggedItem.itemName);
            }

            Debug.Log($"✅ {draggedItem.itemName} placed in {acceptedCategory} slot.");
        }
        else
        {
            Debug.Log($"❌ Wrong category: {draggedItem.category} can't go in {acceptedCategory}.");
        }
    }
}