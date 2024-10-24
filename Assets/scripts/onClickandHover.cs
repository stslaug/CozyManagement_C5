using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyObject : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D hoverCursor;  // Assign your cursor texture in the Inspector
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private void Start()
    {
        Cursor.SetCursor(null, hotSpot, cursorMode);
    }

    public void OnPointerDown(PointerEventData eventData)
    { // Cursor on Click
        UnityEngine.Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    { // Cursor On Hover
        UnityEngine.Debug.Log("Hovered over " + eventData.pointerCurrentRaycast.gameObject.name);
        Cursor.SetCursor(hoverCursor, hotSpot, cursorMode); // Set to cursor ( Assign in inspector)
    }

    public void OnPointerExit(PointerEventData eventData)
    {      // Cursor On Exit 
        Cursor.SetCursor(null, hotSpot, cursorMode); // Reset to default cursor
    }



}
