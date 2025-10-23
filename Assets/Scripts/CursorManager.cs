using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D hoverCursor;

    private Vector2 defaultHotspot;
    private Vector2 hoverHotspot;

    void Start()
    {
        defaultHotspot = new Vector2(defaultCursor.width / 2, defaultCursor.height / 2);
        hoverHotspot = new Vector2(hoverCursor.width / 2, hoverCursor.height / 2);
        Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);

        // Automatically add hover events to all buttons
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = btn.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { OnHoverEnter(); });
            trigger.triggers.Add(enterEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { OnHoverExit(); });
            trigger.triggers.Add(exitEntry);
        }
    }

    void OnHoverEnter()
    {
        Cursor.SetCursor(hoverCursor, hoverHotspot, CursorMode.Auto);
    }

    void OnHoverExit()
    {
        Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
    }
}