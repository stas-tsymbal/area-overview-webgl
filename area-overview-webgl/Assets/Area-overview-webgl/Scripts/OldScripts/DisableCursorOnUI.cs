using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Disable cursor for teleport when enter on ui
 */
public class DisableCursorOnUI : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter (PointerEventData eventData)
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        if(!Application.isMobilePlatform)
            NormalDetector.Instance.DisableCursor();
    }
}
