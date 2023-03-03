using System.Collections;
using System.Collections.Generic;
using Area_overview_webgl.Scripts.ParallelAreaIndicator;
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
            ParallelAreaIndicator.Instance.DisableCursor();
        }
            
    }

    void OnMouseExit()
    {
        if(!Application.isMobilePlatform) 
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
