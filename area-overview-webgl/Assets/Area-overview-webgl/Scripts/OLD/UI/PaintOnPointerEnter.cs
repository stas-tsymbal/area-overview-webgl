using System;
using System.Collections;
using System.Collections.Generic;
using Area_overview_webgl.Scripts.ParallelAreaScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Script paint image when pointer over image
 */
public class PaintOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private void Awake()
    {
        SetMaxColor(false);
    }

    [SerializeField] private Image image;
    public void OnPointerEnter (PointerEventData eventData)
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        
        if(!Application.isMobilePlatform)
            ParallelAreaIndicatorActivationController.Instance.DisableCursor();
        SetMaxColor(true);
    }
    
    public void OnPointerExit (PointerEventData eventData)
    {
        SetMaxColor(false);
    }

    private void SetMaxColor(bool _val)
    {
        if (_val)
        {
            Color32 tempColor = image.color;
            tempColor.a = 225;
            image.color = tempColor;
        }
        else
        {
            Color32 tempColor = image.color;
            tempColor.a = 150;
            image.color = tempColor;
        }
    }
}
