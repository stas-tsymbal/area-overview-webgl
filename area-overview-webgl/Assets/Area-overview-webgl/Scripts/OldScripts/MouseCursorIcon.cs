using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Set new cursor icon when cursor over gameobject 
 */
public class MouseCursorIcon : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    void OnMouseOver()
    {
        if (UiController.Instance.SomeOverlayUiIsActive())
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
            return;
        }
        
        if (!Application.isMobilePlatform )
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            NormalDetector.Instance.DisableCursor();
        }
            
    }

    void OnMouseExit()
    {
        if(!Application.isMobilePlatform) 
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
