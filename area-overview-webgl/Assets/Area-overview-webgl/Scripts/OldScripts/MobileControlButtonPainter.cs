using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

/**
 * This script pain btn on touch
 */
[RequireComponent(typeof(Image))]
public class MobileControlButtonPainter : MonoBehaviour
{
    private Image btn = null;
    [SerializeField] private Color32 standardColor;
    [SerializeField] private Color32 pressedColor;
    
    private void Awake()
    {
      //  btn = gameObject.GetComponent<Image>();
    }

    public void IsPressBtn(bool _val)
    {
     //   btn.color = _val ? pressedColor : standardColor;
    }
    
}
