using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public string acceptedCategory; // e.g. "top", "pants", etc.
    public StyleManager styleManager;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (item != null && item.category == acceptedCategory)
        {
            // Move item into slot
            item.transform.SetParent(transform);
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            // Update style manager
            styleManager.SelectOutfitPiece(item.category, item.itemName);
        }
    }
}