using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Item Info")]
    public string category;
    public string itemName;

    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Canvas canvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move item following the mouse
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // If not dropped onto a DropZone, go back
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent, true);
            transform.localPosition = Vector3.zero;
        }
    }
}