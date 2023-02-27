using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Check platform and activate canvas 
 */
public class CanvasChecker : MonoBehaviour
{
    [SerializeField] private GameObject mobileCanvas;
    private void Awake()
    {
        if (Application.isMobilePlatform)
            mobileCanvas.gameObject.SetActive(true);
    }
}
