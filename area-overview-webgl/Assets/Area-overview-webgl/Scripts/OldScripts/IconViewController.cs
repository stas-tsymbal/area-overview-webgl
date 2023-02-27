using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Icon view controller
 */
public class IconViewController : MonoBehaviour
{
    public static IconViewController Instance;
    [SerializeField] private IconPrefabSample pcIcon;
    [SerializeField] private IconPrefabSample mobileIcon;
    [SerializeField] private GameObject canvasForHide;
    [Header("Set on awake")]
    [SerializeField] private IconPrefabSample currentIconSample;
    
    private bool isMobile;
    [Serializable] 
    private class IconSample // Icon sample data: sprite and text
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string  iconText;

        public Sprite GetSprite()
        {
            return icon;
        }

        public string GetText()
        {
            return iconText;
        }
    }
    // icon for gallery 
    [SerializeField] private List<IconSample> iconSamples = new List<IconSample>();
    
    private void Start()
    {
        Instance = this;
        isMobile = Application.isMobilePlatform;
        currentIconSample = isMobile ? mobileIcon : pcIcon;
        UiController.Instance.SetView(1, currentIconSample.gameObject);
    }
    
    // close this window
    public void Close()
    {
        UiController.Instance.HideOverlayUI(1);
        canvasForHide.SetActive(true);
    }

    // call from icon, _iconNUmber -> index in iconSamples
    public void ShowIcon(int _iconNumber)
    {
        if( UiController.Instance.SomeOverlayUiIsActive()) // block click if icon is open
            return;
        
        if(!Application.isMobilePlatform)
            NormalDetector.Instance.DisableCursor();
        
        currentIconSample.SetImage(iconSamples[_iconNumber].GetSprite()); 
        currentIconSample.SetText(iconSamples[_iconNumber].GetText()); 
        UiController.Instance.ShowOverlayUI(1);
        canvasForHide.SetActive(false);
    }
    
}
